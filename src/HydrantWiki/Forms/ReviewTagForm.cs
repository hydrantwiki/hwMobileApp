using System;
using HydrantWiki.Controls;
using HydrantWiki.Helpers;
using HydrantWiki.Objects;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Forms
{
    public class ReviewTagForm : AbstractPage
    {
        private TagToReview m_Tag;
        private HWHeader m_Header;
        private HWButtonBar m_Bar;
        private HWButton m_Cancel;

        private HWLabel m_User;

        private Grid m_Buttons;
        private HWButton m_Approve;
        private HWButton m_Reject;

        private Grid m_Middle;
        private Map m_Map;
        private Image m_Image;


        private HWLabel m_Nearby;
        private ReviewTagHydrantsListView m_Hydrants;


        public ReviewTagForm() : base("Review Tag")
        {
            m_Header = new HWHeader("Review Tag");
            OutsideLayout.Children.Add(m_Header);

            m_Cancel = new HWButton
            {
                Text = "Cancel",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),

            };
            m_Cancel.Clicked += Cancel_Clicked;
            m_Header.SetLeftButton(m_Cancel);

            AbsoluteLayout layout = new AbsoluteLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            OutsideLayout.Children.Add(layout);

            //Add approve and reject buttons
            m_Buttons = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
            };

            AbsoluteLayout.SetLayoutBounds(m_Buttons, new Rectangle(0, 0, AbsoluteLayout.AutoSize, 50));
            layout.Children.Add(m_Buttons);

            m_Reject = new HWButton
            {
                Text = "Reject",
                WidthRequest = 100,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 10, 10, 10),
                BorderColor = Color.Black,
                BorderWidth = 1,
                BackgroundColor = Color.White
            };
            m_Buttons.Children.Add(m_Reject, 0, 0);

            m_Approve = new HWButton
            {
                Text = "Approve",
                WidthRequest = 100,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(10, 10, 10, 10),
                BorderColor = Color.Black,
                BorderWidth = 1,
                BackgroundColor = Color.White
            };
            m_Buttons.Children.Add(m_Approve, 1, 0);

            //Add the tag's user
            m_User = new HWLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(10, 0, 10, 10)
            };

            AbsoluteLayout.SetLayoutBounds(m_User, new Rectangle(0, 50, AbsoluteLayout.AutoSize, 30));
            layout.Children.Add(m_User);

            //Add Map and Image
            m_Middle = new Grid
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                RowDefinitions = {
                    new RowDefinition { Height = 300 }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                MinimumHeightRequest = 300
            };
            AbsoluteLayout.SetLayoutBounds(m_Middle, new Rectangle(0, 80, AbsoluteLayout.AutoSize, 300));
            layout.Children.Add(m_Middle);

            m_Map = new Map
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            m_Middle.Children.Add(m_Map, 0, 0);

            m_Image = new Image
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            m_Middle.Children.Add(m_Image, 1, 0);

            //Add nearby hydrants
            m_Nearby = new HWLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "Nearby Hydrants",
                Margin = new Thickness(10, 5, 10, 10)
            };
            AbsoluteLayout.SetLayoutBounds(m_Nearby, new Rectangle(0, 380, AbsoluteLayout.AutoSize, 30));
            layout.Children.Add(m_Nearby);

            m_Hydrants = new ReviewTagHydrantsListView
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                HeightRequest = 100
            };
            AbsoluteLayout.SetLayoutBounds(m_Hydrants, new Rectangle(0, 410, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            layout.Children.Add(m_Hydrants);
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }

        public void SetTag(TagToReview _tag)
        {
            m_Tag = _tag;

            m_User.Text = string.Format("User: {0}", _tag.Username);
            m_Hydrants.ItemsSource = _tag.NearbyHydrants;
            m_Image.Source = ImageSource.FromUri(new System.Uri(m_Tag.ImageUrl));

            if (m_Tag.Position != null)
            {
                var pos = new Position(m_Tag.Position.Latitude, m_Tag.Position.Longitude);
                var span = MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(.1));
                m_Map.MoveToRegion(span);
            }
        }
    }
}
