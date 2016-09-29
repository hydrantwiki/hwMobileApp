using System;
using HydrantWiki.Controls;
using Xamarin.Forms;


namespace HydrantWiki.Forms
{
    public class NearbyHydrants : AbstractPage
    {
        private NearbyHydrantsListView m_lstNearby;

        public NearbyHydrants() : base("Nearby Hydrants")
        {
            m_lstNearby = new NearbyHydrantsListView();

            //TODO Start Positioning


            OutsideLayout.Children.Add(m_lstNearby);
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //Stop Positioning
        }
    }
}
