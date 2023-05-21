using StackExchange.Redis;

namespace RedisGeoProcessingDemo.Data
{
    public static class AddDataToRedis
    {
        public static async Task Add(IDatabase db, CityRecord[] cities)
        {
            foreach (var city in cities!)
            {
                var geoEntry = new GeoEntry(Convert.ToDouble(city.Lat), Convert.ToDouble(city.Lng), city.City);

                await db.GeoAddAsync("UK", geoEntry);

                Console.WriteLine($"{city.City} {city.Lat} {city.Lng}");
            }
        }
    }
}
