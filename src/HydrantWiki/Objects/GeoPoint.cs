using System;
namespace HydrantWiki.Objects
{
    public class GeoPoint
    {
        public DateTimeOffset LocationTime { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double? Elevation { get; set; }

        public double? Accuracy { get; set; }

        public double? Speed { get; set; }

    }
}
