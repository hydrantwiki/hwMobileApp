using System;
namespace HydrantWiki.Objects
{
    public class Tag : AbstractObject
    {
        public Guid ImageGuid { get; set; }

        public GeoPoint Position { get; set; }

    }
}
