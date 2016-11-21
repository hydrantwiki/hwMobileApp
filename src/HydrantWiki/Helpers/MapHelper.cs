using HydrantWiki.Objects;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Helpers
{
    public static class MapHelper
    {
        public static GeoBox GetGeoBox(this MapSpan _span)
        {
            if (_span != null)
            {
                var center = _span.Center;
                var halfheightDegrees = _span.LatitudeDegrees / 2;
                var halfwidthDegrees = _span.LongitudeDegrees / 2;

                var left = center.Longitude - halfwidthDegrees;
                var right = center.Longitude + halfwidthDegrees;
                var top = center.Latitude + halfheightDegrees;
                var bottom = center.Latitude - halfheightDegrees;

                GeoBox box = new GeoBox
                {
                    MinLatitude = left,
                    MaxLatitude = right,
                    MinLongitude = bottom,
                    MaxLongitude = top
                };

                return box;
            }

            return null;
        }

        public static GeoPoint GetCenter(this MapSpan _span)
        {
            if (_span != null)
            {
                return new GeoPoint
                {
                    Latitude = _span.Center.Latitude,
                    Longitude = _span.Center.Longitude
                };
            }

            return null;
        }
    }
}
