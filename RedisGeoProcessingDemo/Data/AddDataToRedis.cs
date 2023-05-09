using System.Text.Json;
using StackExchange.Redis;

namespace RedisGeoProcessingDemo.Data
{
    public static class AddDataToRedis
    {
        public static async Task Add(IDatabase db)
        {
            var fileName = "D:\\Develop\\RedisGeoProcessingDemo\\gb.json";
            using FileStream openStream = File.OpenRead(fileName);
            var cities = await JsonSerializer.DeserializeAsync<CityRecord[]>(openStream);

            foreach (var city in cities!)
            {
                var geoEntry = new GeoEntry(Convert.ToDouble(city.Lat), Convert.ToDouble(city.Lng), city.City);

                await db.GeoAddAsync("UK", geoEntry);

                Console.WriteLine($"{city.City} {city.Lat} {city.Lng}");
            }
        }
    }
}
