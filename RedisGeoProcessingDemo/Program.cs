using System.Text.Json;

using RedisGeoProcessingDemo.Data;
using StackExchange.Redis;

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

app.MapGet("/", async (string? lat, string? lng) => await new MapOperations().GetGeoResults(db, lat, lng));

app.MapGet("/places", async (string? location) => await new MapOperations().GetAllPlaces(db, location));

app.Run();




