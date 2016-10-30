using System;
using System.Collections.Generic;
using HydrantWiki.Objects;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Controls
{
    public class ReviewTagMap : Map
    {
        public ReviewTagMap()
        {
            NearbyHydrants = new List<HydrantPin>();
        }

        public List<HydrantPin> NearbyHydrants { get; set; }

        public TagPin TagToReview { get; set; }

    }
}
