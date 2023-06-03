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

app.MapGet("/", async (string? lat, string? lng) => await GetGeoResults(db, lat, lng));

app.MapGet("/places", async (string? location) => await GetAllPlaces(db, location));

app.Run();

async Task<GeoRadiusResult[]> GetGeoResults(IDatabase database, string? lat, string? lng)
{
    if (string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lng))
    {
        lat = "51.454514";
        lng = "-2.587910";
    }

    var geoResults = await database.GeoSearchAsync("UK", Convert.ToDouble(lat), Convert.ToDouble(lng), new GeoSearchCircle(Convert.ToDouble("10"), GeoUnit.Miles));

    return geoResults;
}

// TODO this needs to accept a search pattern
async Task<List<Places>> GetAllPlaces(IDatabase database, string location)
{
    var geoResults = await database.GeoSearchAsync("UK", Convert.ToDouble("54.0840"), Convert.ToDouble("2.8594"), new GeoSearchCircle(Convert.ToDouble("350"), GeoUnit.Miles));

    var places = new List<Places>();

    foreach (var geoResult in geoResults.ToList())
    {
        if (geoResult.Member.ToString().StartsWith(location,StringComparison.OrdinalIgnoreCase))
        {
            var place = new Places()
            {
                City = geoResult.Member.ToString(),
                Distance = geoResult.Distance,
                Lat = geoResult.Position!.Value.Latitude,
                Lng = geoResult.Position.Value.Longitude
            };

            places.Add(place);
        }
    }
    // TODO just return a place name?
    return places;
}
