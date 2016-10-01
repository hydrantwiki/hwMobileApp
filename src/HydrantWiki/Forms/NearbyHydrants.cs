using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using Xamarin.Forms;


namespace HydrantWiki.Forms
{
    public class NearbyHydrants : AbstractPage
    {
        private NearbyHydrantsListView m_lstNearby;
        private LocationManager m_Location;

        public NearbyHydrants() : base("Nearby Hydrants")
        {
            m_lstNearby = new NearbyHydrantsListView();

            OutsideLayout.Children.Add(m_lstNearby);
            m_Location = new LocationManager();

            StartUpdateLocation();
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
                m_lstNearby.ItemsSource = hydrants;
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
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            m_Location.StopListening();
        }
    }
}
