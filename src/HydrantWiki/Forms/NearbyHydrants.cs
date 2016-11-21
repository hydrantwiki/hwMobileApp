using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Constants;
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
        private List<Hydrant> m_Hydrants = null;

        public NearbyHydrants() : base(DisplayConstants.FormNearbyHydrants)
        {
            m_Location = new LocationManager();

            m_lstNearby = new NearbyHydrantsListView
            {
                IsPullToRefreshEnabled = true,
                Margin = new Thickness(0, 10, 0, 10)
            };
            m_lstNearby.RefreshCommand = new Command(StartRefresh);
            m_lstNearby.ItemSelected += Nearby_ItemSelected;
            OutsideLayout.Children.Add(m_lstNearby);

            HWManager.GetInstance().ApiManager.Log(LogLevels.Info,
                               string.Format("NearbyHydrants viewed by {0}", HydrantWikiApp.User.Username));
        }

        private void StartUpdateLocation()
        {
            Task.Factory.StartNew(() => UpdateLocation());
        }

        private void StartRefresh()
        {
            Task.Factory.StartNew(() => Refresh());
        }

        private async Task Refresh()
        {
            await UpdateLocation();

            Device.BeginInvokeOnMainThread(() =>
            {
                m_lstNearby.EndRefresh();
            });
        }

        private async Task UpdateLocation()
        {
            GeoPoint position = await m_Location.GetLocation();

            m_Hydrants = GetHydrants(position);

            HWManager.GetInstance().ApiManager.Log(LogLevels.Info,
                   string.Format("NearbyHydrants refreshed by {0}", HydrantWikiApp.User.Username));

            Device.BeginInvokeOnMainThread(() =>
            {
                m_lstNearby.ItemsSource = m_Hydrants;
            });
        }

        void Nearby_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Hydrant;

            if (item != null)
            {
                HydrantDetailsForm details = new HydrantDetailsForm();
                details.SetHydrant(item);

                Navigation.PushModalAsync(details);
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

            if (m_Hydrants == null)
            {
                StartUpdateLocation();
            } else {
                m_lstNearby.ItemsSource = m_Hydrants;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            m_Location.StopListening();

            m_lstNearby.ItemsSource = null;
        }
    }
}
