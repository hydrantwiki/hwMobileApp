using System;
using HydrantWiki.Controls;
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

        private StackLayout m_Buttons;
        private HWButton m_Approve;
        private HWButton m_Reject;

        private RelativeLayout m_Middle;
        private Map m_Map;
        private Image m_Image;

        private ReviewTagHydrantsListView m_Hydrants;


        public ReviewTagForm() : base("Review Tag")
        {
            m_Header = new HWHeader("Review Tag");
            OutsideLayout.Children.Add(m_Header);

            m_Bar = new HWButtonBar();
            OutsideLayout.Children.Add(m_Bar);

            m_Cancel = m_Bar.Add("Cancel", LayoutOptions.Start);
            m_Cancel.Clicked += Cancel_Clicked;

            //Add approve and reject buttons
            m_Buttons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            OutsideLayout.Children.Add(m_Buttons);

            m_Reject = new HWButton
            {
                Text = "Reject",
                MinimumWidthRequest = 80,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            m_Buttons.Children.Add(m_Reject);



            m_Approve = new HWButton
            {
                Text = "Approve",
                MinimumWidthRequest = 80,
                HorizontalOptions = LayoutOptions.End
            };
            m_Buttons.Children.Add(m_Approve);

            m_User = new HWLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };
            OutsideLayout.Children.Add(m_User);

            //Add Map and Image
            m_Middle = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            OutsideLayout.Children.Add(m_Middle);

            m_Map = new Map
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            m_Middle.Children.Add(
                m_Map,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width / 3;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height / 2;
                })
            );

            m_Image = new Image
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            m_Middle.Children.Add(
                m_Image,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width / 3;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height / 2;
                })
            );

            //Add nearby hydrants
            m_Hydrants = new ReviewTagHydrantsListView
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
            };
            OutsideLayout.Children.Add(m_Hydrants);
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

            if (Device.Idiom == TargetIdiom.Phone)
            {
                m_Image.Source = ImageSource.FromUri(new System.Uri(m_Tag.ThumbnailUrl));
            } else {
                m_Image.Source = ImageSource.FromUri(new System.Uri(m_Tag.ImageUrl));
            }
        }
    }
}
