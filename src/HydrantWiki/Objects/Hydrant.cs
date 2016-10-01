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

        public string DisplayText
        {
            get
            {
                if (Position != null)
                {
                    return string.Format("Latitude: {0:00.000000}, Longitude: {1:00.000000}", Position.Latitude, Position.Longitude);
                }

                return "Unknown Position";
            }
        }

        public string DisplayLatitudeText
        {
            get
            {
                if (Position != null)
                {
                    return string.Format("Latitude: {0:00.000000}", Position.Latitude);
                }

                return "Unknown Latitude";
            }
        }

        public string DisplayLongitudeText
        {
            get
            {
                if (Position != null)
                {
                    return string.Format("Longitude: {0:00.000000}", Position.Longitude);
                }

                return "Unknown Longitude";
            }
        }

        public string DisplayDistanceText
        {
            get
            {
                if (Position != null)
                {
                    return string.Format("Distance: {0:0} feet", DistanceInFeet);
                }

                return "Unknown Distance";
            }
        }
    }
}
