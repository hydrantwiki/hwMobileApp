using System.Collections.Generic;
using HydrantWiki.Objects;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Controls
{
    public class HydrantsMap : Map
    {
        public HydrantsMap()
        {
            NearbyHydrants = new List<HydrantPin>();
        }

        public List<HydrantPin> NearbyHydrants { get; set; }
    }
}
