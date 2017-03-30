using System.Linq;
using DiffWebApi.Core.Models;
using DiffWebApi.Core.Repositories;
using DiffWebApi.Core.Repositories.Abstract;
using Xunit;

namespace DiffWebApi.Tests.Unit
{
    public class DataRepositoryTests
    {
        private readonly IDataRepository _dataRepository;

        public DataRepositoryTests()
        {
            _dataRepository = new ComparisonDataRepository();
        }

        [Fact]
        public void Get_WithNoItems_ShouldReturnNull()
        {
            var pair = _dataRepository.Get(1);
            Assert.Null(pair);
        }

        [Fact]
        public void Get_WithNoSuchItem_ShouldReturnNull()
        {
            var pair = new PositionPair(1);
            _dataRepository.AddOrUpdate(pair);

            var testPair = _dataRepository.Get(2);
            Assert.Null(testPair);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        public void Add_NewAddedPair_ShouldBeInList(int id)
        {
            var pair = new PositionPair(id);
            _dataRepository.AddOrUpdate(pair);

            var testPair = _dataRepository.Get(id);
            Assert.NotNull(testPair);
            Assert.True(testPair.Id == id);
        }

        [Fact]
        public void Update_ShouldBeUpdatedInList()
        {
            var newPair = new PositionPair(100)
            {
                LeftPart = new byte[]{ 0xBE, 0xBE , 0xBE },
                RightPart = new byte[] { 0xCE, 0xCE, 0xCE }

            };
            var pairToUpdate = new PositionPair(100)
            {
                LeftPart = new byte[] { 0xCE, 0xCE, 0xCE },
                RightPart = new byte[] { 0xBE, 0xBE, 0xBE }
            };

            _dataRepository.AddOrUpdate(pairToUpdate);

            var testPair = _dataRepository.Get(pairToUpdate.Id);
            var partsAreEqual = testPair.LeftPart.SequenceEqual(pairToUpdate.LeftPart) && 
                                testPair.RightPart.SequenceEqual(pairToUpdate.RightPart);
            Assert.True(partsAreEqual);
        }

        [Fact]
        public void Delete_ShouldReturnNullAfterDelete()
        {
            var pair = new PositionPair(35);
            _dataRepository.AddOrUpdate(pair);
            _dataRepository.Delete(35);

            var testPair = _dataRepository.Get(35);
            Assert.Null(testPair);
        }
    }
}
