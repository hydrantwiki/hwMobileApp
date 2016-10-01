using System;
using System.Threading.Tasks;
using HydrantWiki.Delegates;
using HydrantWiki.Objects;
using Xamarin.Forms;
using XLabs.Platform.Services.Geolocation;

namespace HydrantWiki.Managers
{
    public class LocationManager : IDisposable
    {
        private IGeolocator m_Locator;

        public event PositionChangedDelegate PositionChanged;

        public LocationManager()
        {
            m_Locator = DependencyService.Get<IGeolocator>();
        }

        public void StartListening()
        {
            if (!m_Locator.IsListening)
            {
                m_Locator.PositionChanged += LocationManager_PositionChanged;
                m_Locator.StartListening(10, 10);
            }
        }

        public void StopListening()
        {
            if (m_Locator.IsListening)
            {
                m_Locator.StopListening();
                m_Locator.PositionChanged -= LocationManager_PositionChanged;
            }
        }

        public bool IsListening
        {
            get
            {
                return m_Locator.IsListening;
            }
        }

        void LocationManager_PositionChanged(object sender, PositionEventArgs e)
        {
            var pc = PositionChanged;
            if (pc != null)
            {
                GeoPoint point = ConvertPosition(e.Position);
                pc(point);
            }
        }

        private GeoPoint ConvertPosition(Position _position)
        {
            GeoPoint point = new GeoPoint
            {
                Latitude = _position.Latitude,
                Longitude = _position.Longitude,
                Altitude = _position.Altitude,
                Accuracy = _position.Accuracy,
                DeviceDateTime = _position.Timestamp,
                Speed = _position.Speed
            };

            return point;
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
                    Altitude = position.Altitude,
                    Accuracy = position.Accuracy,
                    DeviceDateTime = position.Timestamp,
                    Speed = position.Speed
                };

                return point;
            }

            return null;
        }

        public void Dispose()
        {
            StopListening();
        }
    }
}
