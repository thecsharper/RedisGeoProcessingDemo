using RedisGeoProcessingDemo.Data;
using StackExchange.Redis;

var options = ConfigurationOptions.Parse("localhost:6379");
options.Password = "eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81";

var redis = ConnectionMultiplexer.Connect(options);

var db = redis.GetDatabase();

var dataLoaded = false;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
            builder =>
            {
                builder.WithOrigins("https://localhost:7113", "http://localhost:7113", "https://localhost:32419")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true);
            });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", async (string? lat, string? lng) => await GetGeoResults(db, Convert.ToDouble("-2.587910"), Convert.ToDouble("51.454514")));

app.Run();

async Task<GeoRadiusResult[]> GetGeoResults(IDatabase database, double lat, double lng)
{
    if (!dataLoaded)
    {
        await AddDataToRedis.Add(db);
        dataLoaded = true;
    }

    var results = await database.GeoSearchAsync("UK", lng, lat, new GeoSearchCircle(Convert.ToDouble("20.5"), GeoUnit.Miles));

    return results;
}