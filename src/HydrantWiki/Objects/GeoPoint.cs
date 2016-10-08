using System;
namespace HydrantWiki.Objects
{
    public class GeoPoint
    {
        public DateTimeOffset DeviceDateTime { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double? Altitude { get; set; }

        public double? Accuracy { get; set; }

        public double? Speed { get; set; }

        public bool WasAveraged { get; set; }

        public int CountOfPositions { get; set; }
    }
}
