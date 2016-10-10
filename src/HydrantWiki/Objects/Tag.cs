using System;
namespace HydrantWiki.Objects
{
    public class Tag : AbstractObject
    {
        public Guid ImageGuid { get; set; }

        public GeoPoint Position { get; set; }

        public DateTimeOffset TagTime { get; set; }

        public bool SentToServer { get; set; }
    }
}
