using HydrantWiki.Controls;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class HydrantDetailsForm : AbstractPage
    {
        private HWHeader m_Header;
        private HWButtonBar m_ButtonLayout;
        private HWButton CancelButton;

        private Image m_imgMainImage;
        private HWLabel m_lblUsername;
        private HWLabel m_lblLatitude;
        private HWLabel m_lblLongitude;

        public HydrantDetailsForm() : base("Hydrant Details")
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

            Frame imageFrame = new Frame
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HasShadow = false,
                OutlineColor = Color.Black,
                Margin = new Thickness(10, 0, 10, 0)
            };
            OutsideLayout.Children.Add(imageFrame);

            m_imgMainImage = new Image()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFit,
            };
            imageFrame.Content = m_imgMainImage;

            StackLayout labelLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 10)
            };
            OutsideLayout.Children.Add(labelLayout);

            m_lblUsername = new HWLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            labelLayout.Children.Add(m_lblUsername);

            m_lblLatitude = new HWLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            labelLayout.Children.Add(m_lblLatitude);

            m_lblLongitude = new HWLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            labelLayout.Children.Add(m_lblLongitude);
        }

        void CancelButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }

        public void SetHydrant(Hydrant _hydrant)
        {
            if (_hydrant.ImageUrl != null)
            {
                m_imgMainImage.Source = ImageSource.FromUri(new System.Uri(_hydrant.ImageUrl));
            }

            m_lblUsername.Text = string.Format("Username: {0}", _hydrant.Username);

            if (_hydrant.Position != null)
            {
                m_lblLatitude.Text = string.Format("Latitude: {0:00.000000}", _hydrant.Position.Latitude);
                m_lblLongitude.Text = string.Format("Latitude: {0:00.000000}", _hydrant.Position.Longitude);
            }

        }
    }
}
