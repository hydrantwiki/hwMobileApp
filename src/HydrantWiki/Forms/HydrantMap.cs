using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Forms
{
    public class HydrantMap : AbstractPage
    {
        private Map m_Map;
        private LocationManager m_Location;

        public HydrantMap() : base("Hydrant Map")
        {
            m_Location = new LocationManager();

            m_Map = new Map(MapSpan.FromCenterAndRadius(
                new Position(37, -122), Distance.FromMiles(100)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            OutsideLayout.Children.Add(m_Map);
        }

        private void StartUpdateLocation()
        {
            Task t = Task.Factory.StartNew(() => UpdateLocation());
        }

        private async Task UpdateLocation()
        {
            GeoPoint position = await m_Location.GetLocation();

            List<Hydrant> hydrants = GetHydrants(position);

            Device.BeginInvokeOnMainThread(() =>
            {
                if (position != null)
                {
                    var pos = new Position(position.Latitude, position.Longitude);

                    m_Map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1)));

                    if (hydrants != null)
                    {
                        m_Map.Pins.Clear();

                        foreach (var item in hydrants)
                        {
                            if (item.Position != null)
                            {
                                var pin = new Pin()
                                {
                                    Label = "Hydrant"
                                };

                                pin.Position = new Position(item.Position.Latitude, item.Position.Longitude);
                                m_Map.Pins.Add(pin);
                            }
                        }
                    }
                }
            });
        }

        private List<Hydrant> GetHydrants(GeoPoint position)
        {
            if (position != null)
            {
                var response = HWManager.GetInstance().ApiManager.GetHydrantsInCirle(
                    HydrantWikiApp.User,
                    position.Latitude,
                    position.Longitude,
                    500);

                return response.Hydrants;
            }

            return null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            m_Location.StartListening();

            StartUpdateLocation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            m_Location.StopListening();
        }
    }
}
