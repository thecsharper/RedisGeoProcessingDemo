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

            _database.Setup(x => x.GeoSearchAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<GeoSearchShape>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Order>(), It.IsAny<GeoRadiusOptions>(), It.IsAny<CommandFlags>())).Returns(Task.FromResult(geoRadiusResults));

            await new MapOperations().GetAllPlaces(_database!.Object, "Place");

            _database.Verify(x => x.GeoSearchAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<GeoSearchShape>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Order>(), It.IsAny<GeoRadiusOptions>(), It.IsAny<CommandFlags>()), Times.Once());
        }
    }
}


//// Assuming you have a Redis client interface with the GeoSearchAsync method
//public interface IRedisClient
//{
//    Task<string[]> GeoSearchAsync(string key, double longitude, double latitude, double radius);
//}

//// Creating a mock instance of the Redis client
//var redisMock = new Mock<IRedisClient>();

//// Setting up the mock behavior for the GeoSearchAsync method
//redisMock.Setup(client => client.GeoSearchAsync(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
//    .ReturnsAsync(new string[] { "Location1", "Location2", "Location3" });

//// Using the mock in your code
//IRedisClient redisClient = redisMock.Object;
//string[] locations = await redisClient.GeoSearchAsync("myKey", 12.34, 56.78, 1000);

//// Asserting the expected results
//Assert.Equal(3, locations.Length);
//Assert.Contains("Location1", locations);
//Assert.Contains("Location2", locations);
//Assert.Contains("Location3", locations);
