using System;
using HydrantWiki.Controls;
using Xamarin.Forms;

namespace HydrantWiki.Cells
{
    public class ReviewTagCell : ViewCell
    {
        private StackLayout m_Layout;
        private Image m_imgCell;

        private HWLabel m_lblUsername;
        private HWLabel m_lblUserInfo;
        private HWLabel m_lblLatitude;
        private HWLabel m_lblLongitude;
        private HWLabel m_lblDistance;

        public ReviewTagCell()
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

            m_lblUsername = new HWLabel();
            m_lblUsername.SetBinding(Label.TextProperty, "DisplayUsername");
            m_lblUsername.VerticalTextAlignment = TextAlignment.Center;
            m_lblUsername.HorizontalTextAlignment = TextAlignment.Start;
            rows.Children.Add(m_lblUsername);

            m_lblUserInfo = new HWLabel();
            m_lblUserInfo.SetBinding(Label.TextProperty, "UserInfo");
            m_lblUserInfo.VerticalTextAlignment = TextAlignment.Center;
            m_lblUserInfo.HorizontalTextAlignment = TextAlignment.Start;
            rows.Children.Add(m_lblUserInfo);

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

            View = m_Layout;
        }
    }
}
