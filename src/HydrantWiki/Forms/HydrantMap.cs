using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Forms
{
    public class HydrantMap : AbstractPage
    {
        private Map m_Map;

        public HydrantMap() : base("Hydrant Map")
        {
            m_Map = new Map(MapSpan.FromCenterAndRadius(
                new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //TODO Start Positioning


            OutsideLayout.Children.Add(m_Map);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //Stop Positioning
        }
    }
}
