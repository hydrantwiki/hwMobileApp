using System;
namespace HydrantWiki.Objects
{
    public class Hydrant
    {
        public Guid HydrantGuid { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }
        public double? DistanceInFeet { get; set; }
        public GeoPoint Position { get; set; }
        public string Username { get; set; }
    }
}
