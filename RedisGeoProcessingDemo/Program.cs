using RedisGeoProcessingDemo.Data;
using StackExchange.Redis;
using System.Text.Json;

var fileName = "D:\\Develop\\RedisGeoProcessingDemo\\gb.json";

//using FileStream openStream = File.OpenRead(fileName);
//var cities = await JsonSerializer.DeserializeAsync<CityRecord[]>(openStream);

var options = ConfigurationOptions.Parse("localhost:6379");
options.Password = "eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81";

var redis = ConnectionMultiplexer.Connect(options);

var db = redis.GetDatabase();

var key = "1"; 
var value = "A";

db.StringSet(key, value);

value = db.StringGet(key);

Console.WriteLine($"Key value: {value}");

//foreach (var city in cities!)
//{
//    var geoEntry = new GeoEntry(Convert.ToDouble(city.Lat), Convert.ToDouble(city.Lng), city.City);

//    await db.GeoAddAsync("UK", geoEntry);

//   Console.WriteLine($"{city.City} {city.Lat} {city.Lng}");
//}

Console.WriteLine(await db.GeoDistanceAsync("UK", "Bristol", "Charfield", GeoUnit.Miles));
Console.WriteLine(await db.GeoDistanceAsync("UK", "Bristol", "Alford", GeoUnit.Miles));

Console.WriteLine(await db.GeoSearchAsync("UK", );