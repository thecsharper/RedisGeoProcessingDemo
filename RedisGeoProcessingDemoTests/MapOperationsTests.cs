using Moq;
using StackExchange.Redis;
using RedisGeoProcessingDemo.Data;
using FluentAssertions;

namespace RedisGeoProcessingDemoTests
{
    public class MapOperationsTests
    {
        private Mock<IDatabase>? _database;

        [Fact]
        public async Task Get_all_places()
        {
            var postion = new GeoPosition(32.3, 1.11);

            var geoResult = new GeoRadiusResult("UK", 3.23, 0, postion);

            GeoRadiusResult[] geoRadiusResults = { geoResult };

            _database = new Mock<IDatabase>();

            _database.Setup(x => x.GeoSearchAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<GeoSearchShape>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Order>(), It.IsAny<GeoRadiusOptions>(), It.IsAny<CommandFlags>())).Returns(Task.FromResult(geoRadiusResults));

            var result = await new MapOperations().GetAllPlaces(_database!.Object, "Place");

            result.Should().BeOfType<List<string>>();
        }

        [Fact]
        public async Task Get_geo_results()
        {
            var postion = new GeoPosition(32.3, 1.11);

            var geoResult = new GeoRadiusResult("UK", 3.23, 0, postion);

            GeoRadiusResult[] geoRadiusResults = { geoResult };

            _database = new Mock<IDatabase>();

            _database.Setup(x => x.GeoSearchAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<GeoSearchShape>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Order>(), It.IsAny<GeoRadiusOptions>(), It.IsAny<CommandFlags>())).Returns(Task.FromResult(geoRadiusResults));

            var result = await new MapOperations().GetGeoResults(_database!.Object, "1.1","-0.1");

            result.Should().BeOfType<List<Places>>();
        }
    }
}
