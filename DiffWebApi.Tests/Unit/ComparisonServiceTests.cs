using System.Linq;
using DiffWebApi.Core.Models;
using DiffWebApi.Core.Repositories;
using DiffWebApi.Core.Repositories.Abstract;
using DiffWebApi.Core.Services;
using DiffWebApi.Core.Services.Abstract;
using Xunit;

namespace DiffWebApi.Tests.Unit
{
    public class ComparisonServiceTests
    {
        private readonly IComparisonService _comparisonService;
        private readonly IDataRepository _dataRepository;

        public ComparisonServiceTests()
        {
            _dataRepository = new ComparisonDataRepository();
            _comparisonService = new ComparisonService(_dataRepository);
        }

        [Fact]
        public void Add_NewAddedPair_ShouldBeInList()
        {
            const int id = 1;
            var pair = new PositionPair(id)
            {
                LeftPart = new byte[] { 0xBE, 0xBE, 0xBE }
            };
            _comparisonService.AddOrUpdateData(pair.Id, PositionType.Left, pair.LeftPart);

            var testPair = _dataRepository.Get(pair.Id);
            Assert.NotNull(testPair);
            Assert.True(testPair.LeftPart.SequenceEqual(pair.LeftPart));
        }

        [Fact]
        public void Update_ShouldBeUpdatedInList()
        {
            var newPair = new PositionPair(100)
            {
                RightPart = new byte[] { 0xBE, 0xBE, 0xBE }

            };
            var newRightPart = new byte[] { 0xCE, 0xCE, 0xCE };
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Right, newPair.RightPart);
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Right, newRightPart);

            var testPair = _dataRepository.Get(newPair.Id);
            Assert.True(testPair.RightPart.SequenceEqual(newRightPart));
        }

        [Fact]
        public void Compare_Nulls_AreEqual()
        {
            var newPair = new PositionPair(10);
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Left, null);
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Right, null);

            var comparisonResult = _comparisonService.Compare(newPair.Id);
            Assert.Equal(new ComparisonResult(EqualityType.Equals), comparisonResult);
        }

        [Fact]
        public void Compare_TheSameContent_IsEqual()
        {
            var newPair = new PositionPair(10);
            var hello = new byte[] {0x48, 0x65, 0x6c, 0x6c, 0x6f };
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Left, hello);
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Right, hello);

            var comparisonResult = _comparisonService.Compare(newPair.Id);
            Assert.Equal(new ComparisonResult(EqualityType.Equals), comparisonResult);
        }

        [Fact]
        public void Compare_TheSameLength_NotEqual()
        {
            var newPair = new PositionPair(10);
            var hello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var herro = new byte[] { 0x48, 0x65, 0x72, 0x72, 0x6f };
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Left, hello);
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Right, herro);

            var comparisonResult = _comparisonService.Compare(newPair.Id);
            Assert.Equal(EqualityType.ContentDoNotMatch, comparisonResult.Equality);
        }

        [Fact]
        public void Compare_TheSameLength_CorrectDiff()
        {
            var newPair = new PositionPair(10);
            var hello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var herro = new byte[] { 0x48, 0x65, 0x72, 0x72, 0x6f };
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Left, hello);
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Right, herro);

            var comparisonResult = _comparisonService.Compare(newPair.Id);
            var diff = comparisonResult.Diff.FirstOrDefault();
            Assert.NotNull(comparisonResult.Diff);
            Assert.NotNull(diff);
            Assert.True(diff.Offset == 2);
            Assert.True(diff.Length == 2);
        }

        [Fact]
        public void Compare_DifferentLength_NotEqual()
        {
            var newPair = new PositionPair(10);
            var hello = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var helloWorld = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x2c, 0x20, 0x77, 0x6f, 72, 0x6c, 0x64 };
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Left, hello);
            _comparisonService.AddOrUpdateData(newPair.Id, PositionType.Right, helloWorld);

            var comparisonResult = _comparisonService.Compare(newPair.Id);
            Assert.Equal(new ComparisonResult(EqualityType.SizeDoNotMatch), comparisonResult);
        }
    }
}