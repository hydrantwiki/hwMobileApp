using HydrantWiki.Objects;

namespace HydrantWiki.Helpers
{
    public static class GeoHelper
    {
        public static GeoPoint Center(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = (_box.MaxLatitude - _box.MinLatitude) / 2;
                output.Longitude = (_box.MaxLongitude - _box.MinLongitude) / 2;

                return output;
            }

            return null;
        }

        public static GeoPoint North(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = _box.MaxLatitude;
                output.Longitude = (_box.MaxLongitude - _box.MinLongitude) / 2;

                return output;
            }

            return null;
        }

        public static GeoPoint NorthEast(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = _box.MaxLatitude;
                output.Longitude = _box.MaxLongitude;

                return output;
            }

            return null;
        }

        public static GeoPoint East(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = (_box.MaxLatitude - _box.MinLatitude) / 2;
                output.Longitude = _box.MaxLongitude;

                return output;
            }

            return null;
        }

        public static GeoPoint SouthEast(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = _box.MinLatitude;
                output.Longitude = _box.MaxLongitude;

                return output;
            }

            return null;
        }

        public static GeoPoint South(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = _box.MinLatitude;
                output.Longitude = (_box.MaxLongitude - _box.MinLongitude) / 2;

                return output;
            }

            return null;
        }

        public static GeoPoint SouthWest(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = _box.MinLatitude;
                output.Longitude = _box.MinLongitude;

                return output;
            }

            return null;
        }

        public static GeoPoint West(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = (_box.MaxLatitude - _box.MinLatitude) / 2;
                output.Longitude = _box.MinLongitude;

                return output;
            }

            return null;
        }

        public static GeoPoint NorthWest(this GeoBox _box)
        {
            if (_box != null)
            {
                GeoPoint output = new GeoPoint();

                output.Latitude = _box.MaxLatitude;
                output.Longitude = _box.MinLongitude;

                return output;
            }

            return null;
        }
    }
}
