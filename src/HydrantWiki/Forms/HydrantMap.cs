using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Helpers;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Forms
{
    public class HydrantMap : AbstractPage
    {
        private bool m_Loading = false;
        private Map m_Map;
        private LocationManager m_Location;

        public HydrantMap() : base("Hydrant Map")
        {
            m_Loading = true;

            m_Location = new LocationManager();

            m_Map = new Map(MapSpan.FromCenterAndRadius(
                new Position(37, -122), Distance.FromMiles(100)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            m_Map.PropertyChanged += Map_PropertyChanged;

            OutsideLayout.Children.Add(m_Map);

            m_Loading = false;
        }

        private void StartUpdateLocation()
        {
            Task t = Task.Factory.StartNew(() => UpdateLocation());
        }

        void Map_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!m_Loading
                && e.PropertyName == "VisibleRegion")
            {
                Task t = Task.Factory.StartNew(() => UpdateCurrentView());
            }
        }

        private void UpdateCurrentView()
        {
            GeoBox box = m_Map.VisibleRegion.GetGeoBox();

            if (box != null)
            {
                List<Hydrant> hydrants = GetHydrants(box);

                Device.BeginInvokeOnMainThread(() =>
                {
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
                });
            }
        }

        private async Task UpdateLocation()
        {
            m_Loading = true;

            GeoPoint position = await m_Location.GetLocation();
            if (position != null)
            {
                var pos = new Position(position.Latitude, position.Longitude);
                var span = MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1));
                var box = span.GetGeoBox();

                List<Hydrant> hydrants = GetHydrants(box);

                Device.BeginInvokeOnMainThread(() =>
                {
                    m_Map.MoveToRegion(span);

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

                    m_Loading = false;
                });
            }
        }

        private List<Hydrant> GetHydrants(GeoBox box)
        {
            if (box != null)
            {
                var response = HWManager.GetInstance().ApiManager.GetHydrantsInBox(
                    HydrantWikiApp.User,
                    box.MinLatitude,
                    box.MaxLatitude,
                    box.MinLongitude,
                    box.MaxLongitude);

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
