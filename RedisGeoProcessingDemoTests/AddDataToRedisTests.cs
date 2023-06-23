using Moq;
using StackExchange.Redis;
using RedisGeoProcessingDemo.Data;

namespace RedisGeoProcessingDemoTests
{
    public class AddDataToRedisTests
    {
        private Mock<IDatabase>? _database;

        [Fact]
        public async Task Add_geo_data_to_database()
        {

            CityRecord[] cityRecords = { new CityRecord() { Lat = "41.1", Lng = "-2.0" } };
            
            _database = new Mock<IDatabase>();

            _database!
                .Setup(x => x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            await AddDataToRedis.Add(_database!.Object, cityRecords);

            _database.Verify(x => x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>()), Times.Once());
        }
    }
}

