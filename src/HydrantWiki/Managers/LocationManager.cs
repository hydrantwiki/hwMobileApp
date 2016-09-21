using System;
using System.Threading.Tasks;
using HydrantWiki.Objects;
using Xamarin.Forms;
using XLabs.Platform.Services.Geolocation;

namespace HydrantWiki.Managers
{
    public class LocationManager
    {
        private IGeolocator m_Locator;

        public LocationManager()
        {
            m_Locator = DependencyService.Get<IGeolocator>();
        }

        public async Task<GeoPoint> GetLocation()
        {
            var position = await m_Locator.GetPositionAsync(10000);

            if (position != null)
            {
                GeoPoint point = new GeoPoint
                {
                    Latitude = position.Latitude,
                    Longitude = position.Longitude,
                    Elevation = position.Altitude,
                    Accuracy = position.Accuracy,
                    LocationTime = position.Timestamp,
                    Speed = position.Speed
                }
            }

            return null;
        }
    }
}
