namespace RedisGeoProcessingDemo.Data
{
    public class Places
    {
        public string? City { get; set; }

        public double? Distance { get; set; }

        public Position? Position { get; set; }
    }

    public class Position
    {
        public string? Lat { get; set; }

        public string? Lng { get; set; }
    }
}
