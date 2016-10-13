using System;
using LiteDB;

namespace HydrantWiki.Objects
{
    public class Tag : AbstractObject
    {
        public Guid ImageGuid { get; set; }

        public GeoPoint Position { get; set; }

        public DateTimeOffset TagTime { get; set; }

        public bool SentToServer { get; set; }

        [BsonIgnore]
        public string DisplaySentToServer
        {
            get
            {
                if (SentToServer)
                {
                    return "Submitted to Server";
                } else
                {
                    return "Awaiting delivery to Server";
                }
            }
        }

        [BsonIgnore]
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

        [BsonIgnore]
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
    }
}
