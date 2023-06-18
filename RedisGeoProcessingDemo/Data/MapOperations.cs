using StackExchange.Redis;

namespace RedisGeoProcessingDemo.Data
{
    public class MapOperations
    {
        public async Task<List<string>> GetAllPlaces(IDatabase database, string? location)
        {
            var geoResults = await database.GeoSearchAsync("UK", Convert.ToDouble("54.0840"), Convert.ToDouble("2.8594"), new GeoSearchCircle(Convert.ToDouble("350"), GeoUnit.Miles));

            var placeItem = new List<string>();

            foreach (var geoResult in geoResults.ToList())
            {
                if (geoResult.Member.ToString().StartsWith(location!, StringComparison.OrdinalIgnoreCase))
                {
                    var place = new Places()
                    {
                        City = geoResult.Member.ToString().Trim(),
                        Lat = geoResult.Position!.Value.Latitude,
                        Lng = geoResult.Position!.Value.Longitude
                    };

                    placeItem.Add($"<li onClick = \"selectCountry('{place.City}');selectMap($(this).data('lat'),$(this).data('lng'));\" data-lat=\"{place.Lat}\" data-lng=\"{place.Lng}\">{place.City}</li>");
                }
            }

            return placeItem;
        }

       public async Task<GeoRadiusResult[]> GetGeoResults(IDatabase database, string? lat, string? lng)
        {
            // Default location
            if (string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lng))
            {
                lat = "51.454514";
                lng = "-2.587910";
            }

            var geoResults = await database.GeoSearchAsync("UK", Convert.ToDouble(lat), Convert.ToDouble(lng), new GeoSearchCircle(Convert.ToDouble("10"), GeoUnit.Miles));

            return geoResults;
        }
    }
}
