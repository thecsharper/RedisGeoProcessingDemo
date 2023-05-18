using RedisGeoProcessingDemo.Data;
using StackExchange.Redis;
using System.Text.Json;

var fileName = "D:\\Develop\\RedisGeoProcessingDemo\\gb.json";
var options = ConfigurationOptions.Parse("localhost:6379");
options.Password = "eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81";

var redis = ConnectionMultiplexer.Connect(options);

var db = redis.GetDatabase();

using FileStream openStream = File.OpenRead(fileName);
var cities = await JsonSerializer.DeserializeAsync<CityRecord[]>(openStream);

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

app.MapGet("/load", async () => await AddDataToRedis.Add(db, cities!));

app.MapGet("/", async (string? lat, string? lng) => await GetGeoResults(db, lat, lng));

app.Run();

async Task<GeoRadiusResult[]> GetGeoResults(IDatabase database, string? lat, string? lng)
{
    if(string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lng)) 
    {
        lat = "51.454514";
        lng = "-2.587910";
    }

    var results = await database.GeoSearchAsync("UK", Convert.ToDouble(lat), Convert.ToDouble(lng), new GeoSearchCircle(Convert.ToDouble("300.5"), GeoUnit.Miles));

    return results;
}