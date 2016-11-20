using System;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class HWHeader : ContentView
    {
        private HWLabel m_lblTitle;
        private AbsoluteLayout m_Header;

        public HWHeader(string _title)
        {
            m_Header = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex(UIConstants.NavBarColor),
                Margin = new Thickness(0, 0, 0, 0),
                MinimumHeightRequest = 65,
                HeightRequest = 65
            };
            Content = m_Header;

            m_lblTitle = new HWLabel
            {
                Text = _title,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(UIConstants.NavBarTextColor),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            AbsoluteLayout.SetLayoutFlags(m_lblTitle,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(m_lblTitle,
                new Rectangle(0.5, 0.7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            m_Header.Children.Add(m_lblTitle);

            BoxView line = new BoxView
            {
                HeightRequest = 1,
                Color = Color.FromHex(UIConstants.HeaderLineColor),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutFlags(
                line,
                AbsoluteLayoutFlags.WidthProportional);

            AbsoluteLayout.SetLayoutBounds(line,
                new Rectangle(0,
                               64.5,
                               1,
                               AbsoluteLayout.AutoSize));

            m_Header.Children.Add(line);
        }

        public void SetLeftButton(HWButton button)
        {
            AbsoluteLayout.SetLayoutBounds(
                button,
                new Rectangle(0, 30, 70, 25));
            m_Header.Children.Add(button);
        }

        public void SetRightButton(HWButton button)
        {
            int left = HydrantWikiApp.ScreenWidth - 70;

            AbsoluteLayout.SetLayoutBounds(
                button,
                new Rectangle(left, 30, 70, 25));
            m_Header.Children.Add(button);
        }
    }
}
