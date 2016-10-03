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
            m_Location = new LocationManager();

            m_lstNearby = new NearbyHydrantsListView();
            m_lstNearby.ItemSelected += Nearby_ItemSelected;
            OutsideLayout.Children.Add(m_lstNearby);
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

        void Nearby_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Hydrant;

            if (item != null)
            {
                HydrantDetailsForm details = new HydrantDetailsForm();
                details.SetHydrant(item);

                Navigation.PushAsync(details);
            }
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

            m_lstNearby.ItemsSource = null;
        }
    }
}
