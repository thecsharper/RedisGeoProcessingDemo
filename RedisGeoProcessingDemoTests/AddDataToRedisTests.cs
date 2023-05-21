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

            CityRecord[] cityRecords = { };
          
            _database!.Setup(x => x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);

            // Act
            await AddDataToRedis.Add(_database.Object, cityRecords);

            // Assert
            _database.Verify(x=>x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>()), Times.Once());
        }
    }
}