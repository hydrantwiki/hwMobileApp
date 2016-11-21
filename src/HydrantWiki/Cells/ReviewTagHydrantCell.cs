using System;
using HydrantWiki.Controls;
using HydrantWiki.Delegates;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Cells
{
    public class ReviewTagHydrantCell : ViewCell
    {
        private StackLayout m_Layout;
        private Image m_imgCell;

        private HWLabel m_lblLatitude;
        private HWLabel m_lblLongitude;
        private HWLabel m_lblDistance;

        public static event HydrantMatchDelegate HydrantMatchClicked;

        public ReviewTagHydrantCell()
        {
            m_Layout = new StackLayout()
            {
                Padding = new Thickness(2, 2),
                Margin = new Thickness(20, 5, 20, 5),
                Orientation = StackOrientation.Horizontal
            };

            m_imgCell = new Image
            {
                Aspect = Aspect.AspectFit,
                WidthRequest = 70,
                HeightRequest = 70
            };
            m_imgCell.SetBinding(Image.SourceProperty, "ThumbnailUrl");
            m_Layout.Children.Add(m_imgCell);

            StackLayout rows = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            m_Layout.Children.Add(rows);


            m_lblLatitude = new HWLabel();
            m_lblLatitude.SetBinding(Label.TextProperty, "DisplayLatitudeText");
            m_lblLatitude.VerticalTextAlignment = TextAlignment.Center;
            m_lblLatitude.HorizontalTextAlignment = TextAlignment.Start;
            rows.Children.Add(m_lblLatitude);

            m_lblLongitude = new HWLabel();
            m_lblLongitude.SetBinding(Label.TextProperty, "DisplayLongitudeText");
            m_lblLongitude.VerticalTextAlignment = TextAlignment.Center;
            m_lblLongitude.HorizontalTextAlignment = TextAlignment.Start;
            rows.Children.Add(m_lblLongitude);

            m_lblDistance = new HWLabel();
            m_lblDistance.SetBinding(Label.TextProperty, "DisplayDistanceText");
            m_lblDistance.VerticalTextAlignment = TextAlignment.Center;
            m_lblDistance.HorizontalTextAlignment = TextAlignment.Start;
            rows.Children.Add(m_lblDistance);

            var matchAction = new MenuItem { Text = "Match", IsDestructive = false }; // red background
            matchAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            matchAction.Clicked += MatchClicked;
            ContextActions.Add(matchAction);

            View = m_Layout;
        }


        void MatchClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var matchClicked = HydrantMatchClicked;

            if (matchClicked != null)
            {
                Hydrant hydrant = (Hydrant)menuItem.CommandParameter;
                matchClicked(hydrant.HydrantGuid);
            }
        }
    }
}
