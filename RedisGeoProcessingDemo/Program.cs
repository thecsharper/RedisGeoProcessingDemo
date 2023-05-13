using RedisGeoProcessingDemo.Data;
using StackExchange.Redis;

var options = ConfigurationOptions.Parse("localhost:6379");
options.Password = "eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81";

var redis = ConnectionMultiplexer.Connect(options);

var db = redis.GetDatabase();

await AddDataToRedis.Add(db);

var key = "1"; 
var value = "A";

db.StringSet(key, value);

value = db.StringGet(key);

Console.WriteLine($"Key value: {value}");

Console.WriteLine(await db.GeoDistanceAsync("UK", "Bristol", "Charfield", GeoUnit.Miles));
Console.WriteLine(await db.GeoDistanceAsync("UK", "Bristol", "Alford", GeoUnit.Miles));

var result = await db.GeoSearchAsync("UK", Convert.ToDouble("51.454514"), Convert.ToDouble("-2.587910"), new GeoSearchCircle(Convert.ToDouble("20.5"), GeoUnit.Miles));

//foreach (var item in result)
//{
//    Console.WriteLine($"{item.Member} {item.Position} {item.Distance} miles");
//}


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:7113", "http://localhost:7113", "https://localhost:32419").AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true);
                      });

});
var app = builder.Build();

app.UseCors();
app.MapGet("/", () => result.ToList());

app.Run();