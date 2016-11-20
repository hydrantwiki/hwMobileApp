using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Helpers;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class HydrantDetailsForm : AbstractPage
    {
        private HWHeader m_Header;
        private HWButton CancelButton;

        private Image m_imgMainImage;
        private HWLabel m_lblUsername;
        private HWLabel m_lblLatitude;
        private HWLabel m_lblLongitude;

        public HydrantDetailsForm() : base(DisplayConstants.FormHydrantDetails)
        {
            m_Header = new HWHeader(DisplayConstants.FormHydrantDetails)
            {
                Margin = new Thickness(0, 0, 0, 0)
            };
            OutsideLayout.Children.Add(m_Header);

            CancelButton = new HWButton
            {
                Text = DisplayConstants.Cancel,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
            };
            m_Header.SetLeftButton(CancelButton);
            CancelButton.Clicked += CancelButton_Clicked;

            Frame imageFrame = new Frame
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HasShadow = false,
                OutlineColor = Color.Black,
                HeightRequest = HydrantWikiApp.ScreenHeight - 200,
                WidthRequest = HydrantWikiApp.ScreenWidth - 20,
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

            m_lblUsername.Text = string.Format(DisplayConstants.UsernameDisplay, _hydrant.Username);

            if (_hydrant.Position != null)
            {
                m_lblLatitude.Text = _hydrant.Position.Latitude.AsLatitude();
                m_lblLongitude.Text = _hydrant.Position.Longitude.AsLongitude();
            }

        }
    }
}
