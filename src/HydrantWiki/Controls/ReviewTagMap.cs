﻿using System;
using System.Collections.Generic;
using HydrantWiki.Objects;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Controls
{
    public class ReviewTagMap : Map
    {
        public List<Hydrant> NearbyHydrants { get; set; }

        public Tag TagToReview { get; set; }

    }
}
