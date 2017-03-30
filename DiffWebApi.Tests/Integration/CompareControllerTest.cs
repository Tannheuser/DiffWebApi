using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiffWebApi.Core.Models;
using DiffWebApi.Web;
using DiffWebApi.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace DiffWebApi.Tests.Integration
{
    public class CompareControllerTest
    {
        private const string ControllerRoute = "/v1/diff/";
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public CompareControllerTest()
        {
           
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task AddPositionData_Test()
        {
            var leftHello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var response = await PostData(PositionType.Left, leftHello);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task TheSameContent_IsEqual()
        {
            var leftHello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var rightHello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            await PostData(PositionType.Left, leftHello);
            await PostData(PositionType.Right, rightHello);

            var response = await CallCompare();
            var result = await GetResponseResult(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.DiffResultType, "Equals");
        }

        [Fact]
        public async Task Compare_TheSameLength_NotEqual()
        {
            var hello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var herro = new byte[] { 0x48, 0x65, 0x72, 0x72, 0x6f };
            await PostData(PositionType.Left, hello);
            await PostData(PositionType.Right, herro);

            var response = await CallCompare();
            var result = await GetResponseResult(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.DiffResultType, "ContentDoNotMatch");
        }

        [Fact]
        public async Task Compare_TheSameLength_CorrectDiff()
        {
            var hello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var herro = new byte[] { 0x48, 0x65, 0x72, 0x72, 0x6f };
            await PostData(PositionType.Left, hello);
            await PostData(PositionType.Right, herro);

            var response = await CallCompare();
            var result = await GetResponseResult(response);
            var diff = result.Diffs.FirstOrDefault();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(diff);
            Assert.True(diff.Offset == 2);
            Assert.True(diff.Length == 2);
        }

        [Fact]
        public async Task Compare_DifferentLength_NotEqual()
        {
            var hello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var helloWorld = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x2c, 0x20, 0x77, 0x6f, 72, 0x6c, 0x64 };
            await PostData(PositionType.Left, hello);
            await PostData(PositionType.Right, helloWorld);

            var response = await CallCompare();
            var result = await GetResponseResult(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.DiffResultType, "SizeDoNotMatch");
        }

        private async Task<HttpResponseMessage> CallCompare()
        {
            var url = $"{ControllerRoute}1";
            return await _client.GetAsync(url);
        }

        private async Task<HttpResponseMessage> PostData(PositionType position, byte[] data)
        {
            var base64Dto = new Base64Dto(data);
            var sendData = PrepareDataToSend(base64Dto);
            string url;

            switch (position)
            {
                case PositionType.Left:
                    url = $"{ControllerRoute}1/left";
                    break;
                case PositionType.Right:
                    url = $"{ControllerRoute}1/right";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }

            Console.Write(url);
            return await _client.PostAsync(url, sendData);
        }

        private StringContent PrepareDataToSend(Base64Dto data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            return new StringContent(jsonData)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("text/json") }
            };
        }

        private async Task<ComparisonResultDto> GetResponseResult(HttpResponseMessage response)
        {
            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ComparisonResultDto>(message);

            return result;
        }
    }
}

