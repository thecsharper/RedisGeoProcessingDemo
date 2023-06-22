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
            CityRecord[] cityRecords = { new CityRecord() { Lat = "41.1", Lng = "-2.0" } };

            var mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            var mockDatabase = new Mock<IDatabase>();
            mockDatabase
                .Setup(x => x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            mockConnectionMultiplexer
                .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);

            await AddDataToRedis.Add(mockDatabase!.Object, cityRecords);
        }
    }
}

// Act


// Arrange

//var key = new RedisKey("test");

//CityRecord[] cityRecords = { new CityRecord() { Lat = "41.1", Lng = "-2.0" } };

//var geoEntry = new GeoEntry(Convert.ToDouble(51.1), Convert.ToDouble(-1.12), new RedisValue("City"));

//_database!.Setup(x => x.GeoAddAsync(key, geoEntry, CommandFlags.None)).ReturnsAsync(true);

//// Act
//await AddDataToRedis.Add(_database.Object, cityRecords);

//// Assert
//_database.Verify(x=>x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry>(), It.IsAny<CommandFlags>()), Times.Once());

//

//public class RedisService
//        {
//            private readonly IConnectionMultiplexer _redisConnection;

//            public RedisService(IConnectionMultiplexer redisConnection)
//            {
//                _redisConnection = redisConnection;
//            }

//            public virtual Task<bool> GeoAddAsync(string key, double longitude, double latitude, string member)
//            {
//                var db = _redisConnection.GetDatabase();
//                return db.GeoAddAsync(key, longitude, latitude, member);
//            }
//        }

//        public class RedisServiceTests
//        {
//            public async Task GeoAddAsync_ShouldReturnTrue()
//            {
//                // Arrange
//                var mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
//                var mockDatabase = new Mock<IDatabase>();
//                mockDatabase
//                    .Setup(x => x.GeoAddAsync(It.IsAny<RedisKey>(), It.IsAny<GeoEntry[]>(), It.IsAny<CommandFlags>()))
//                    .ReturnsAsync(true); 

//                mockConnectionMultiplexer
//                    .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
//                    .Returns(mockDatabase.Object); // Mock GetDatabase method to return the mock database

//                var redisService = new RedisService(mockConnectionMultiplexer.Object);

//                // Act
//                var result = await redisService.GeoAddAsync("key", 1.23, 4.56, "member");

//                // Assert
//                Assert.True(result);
//            }
//        }


//    }
//}