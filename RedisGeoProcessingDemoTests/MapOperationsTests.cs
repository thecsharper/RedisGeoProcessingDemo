using Moq;
using StackExchange.Redis;
using RedisGeoProcessingDemo.Data;

namespace RedisGeoProcessingDemoTests
{
    public  class MapOperationsTests
    {
            private Mock<IDatabase>? _database;

        [Fact]
        public async Task Get_all_places()
        {

             GeoRadiusResult[] geoRadiusResults = { new GeoRadiusResult() { } };
            
            _database = new Mock<IDatabase>();

            _database!
                .Setup(x => x.GeoSearchAsync("UK", Convert.ToDouble("54.0840"), Convert.ToDouble("2.8594"), new GeoSearchCircle(Convert.ToDouble("350"), GeoUnit.Miles)))
                .ReturnsAsync(geoRadiusResults);
                
            await new MapOperations().GetAllPlaces(_database!.Object, "Place");

           // _database.Verify(x => x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>()), Times.Once());
        }
    }
}
