using System;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class HWHeader : ContentView
    {
        private HWLabel m_lblTitle;

        public HWHeader(string _title)
        {
            AbsoluteLayout header = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex(UIConstants.NavBarColor),
                Margin = new Thickness(0, 0, 0, 0),
                MinimumHeightRequest = 65,
                HeightRequest = 65
            };
            Content = header;

            m_lblTitle = new HWLabel
            {
                Text = _title,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(UIConstants.NavBarTextColor),
                FontAttributes = FontAttributes.Bold
            };

            AbsoluteLayout.SetLayoutFlags(m_lblTitle,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(m_lblTitle,
                new Rectangle(0.5,
                    0.7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            header.Children.Add(m_lblTitle);

            BoxView line = new BoxView
            {
                HeightRequest = 1,
                Color = Color.FromHex(UIConstants.LineColor),
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

            header.Children.Add(line);
        }
    }
}
