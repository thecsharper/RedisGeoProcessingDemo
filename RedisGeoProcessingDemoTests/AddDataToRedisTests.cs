using Moq;
using FluentAssertions;
using StackExchange.Redis;
using RedisGeoProcessingDemo.Data;

namespace RedisGeoProcessingDemoTests
{ 
    public class AddDataToRedisTests
    {
        private readonly Mock<IDatabase>? _database;

        [Fact]
        public async Task Add_geo_data_to_database()
        {
            // Arrange

            var key = new RedisKey("test");

            CityRecord[] cityRecords = { new CityRecord() { Lat = "41.1", Lng = "-2.0" } };

            var geoEntry = new GeoEntry(Convert.ToDouble(51.1), Convert.ToDouble(-1.12), "city");

            _database!.Setup(x => x.GeoAddAsync(key, geoEntry, CommandFlags.None)).ReturnsAsync(true);

            // Act
            await AddDataToRedis.Add(_database.Object, cityRecords);

            // Assert
            _database.Verify(x=>x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>()), Times.Once());
        }
    }
}