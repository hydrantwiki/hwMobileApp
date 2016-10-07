using System;
using HydrantWiki.Controls;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class TagHydrant : AbstractPage
    {
        private HWHeader m_Header;
        private HWButtonBar m_ButtonLayout;
        private HWButton CancelButton;
        private HWButton SaveButton;

        private StackLayout m_layoutPhoto;
        private Image m_imgHydrant;
        private HWButton m_btnTakePhoto;
        private HWLabel m_lblCount;
        private HWLabel m_lblLatitude;
        private HWLabel m_lblLongitude;

        public TagHydrant()
            : base("Tag Hydrant")
        {
            m_Header = new HWHeader("Hydrant Details")
            {
                Margin = new Thickness(0, 0, 0, 0)
            };
            OutsideLayout.Children.Add(m_Header);

            m_ButtonLayout = new HWButtonBar();
            OutsideLayout.Children.Add(m_ButtonLayout);

            CancelButton = m_ButtonLayout.Add("Cancel", LayoutOptions.StartAndExpand);
            CancelButton.Clicked += CancelButton_Clicked;

            SaveButton = m_ButtonLayout.Add("Save", LayoutOptions.EndAndExpand);
            SaveButton.Clicked += SaveButton_Clicked;

            m_layoutPhoto = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(5, 0, 5, 0)
            };
            OutsideLayout.Children.Add(m_layoutPhoto);

            Frame imageFrame = new Frame
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                OutlineColor = Color.Black,
                HasShadow = false
            };
            m_layoutPhoto.Children.Add(imageFrame);

            m_imgHydrant = new Image
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFit
            };
            imageFrame.Content = m_imgHydrant;

            m_btnTakePhoto = new HWButton
            {
                Text = "Take Photo",
                WidthRequest = 80,
                BorderColor = Color.Black,

            };
            m_btnTakePhoto.Clicked += TakePhoto_Clicked;
            m_layoutPhoto.Children.Add(m_btnTakePhoto);

            StackLayout lableLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(5, 10, 5, 10)
            };
            OutsideLayout.Children.Add(lableLayout);

            m_lblCount = new HWLabel
            {
                Text = "Position Count:",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            lableLayout.Children.Add(m_lblCount);

            m_lblLatitude = new HWLabel
            {
                Text = "Latitude:",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            lableLayout.Children.Add(m_lblLatitude);

            m_lblLongitude = new HWLabel
            {
                Text = "Longitude:",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            lableLayout.Children.Add(m_lblLongitude);
        }

        void TakePhoto_Clicked(object sender, EventArgs e)
        {

        }

        void CancelButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }

        void SaveButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
