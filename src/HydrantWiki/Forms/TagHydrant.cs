using System;
using System.Threading.Tasks;
using HydrantWiki.Controls;
using HydrantWiki.Helpers;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using HydrantWiki.Workers;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace HydrantWiki.Forms
{
    public class TagHydrant : AbstractPage
    {
        private IMediaPicker m_MediaPicker;
        private readonly TaskScheduler m_Scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private LocationManager m_Location;
        private PositionAverager m_Averager;

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
            m_MediaPicker = DependencyService.Get<IMediaPicker>();
            var device = Resolver.Resolve<IDevice>();
            m_MediaPicker = m_MediaPicker ?? device.MediaPicker;


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
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 80,
                HeightRequest = 40,
                BorderColor = Color.Black,
                BorderWidth = 1,
                BackgroundColor = Color.White
            };
            m_btnTakePhoto.Clicked += TakePhoto_Clicked;
            m_layoutPhoto.Children.Add(m_btnTakePhoto);

            if (!m_MediaPicker.IsCameraAvailable)
            {
                m_btnTakePhoto.IsEnabled = false;
            }

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

            m_Location = new LocationManager();
            m_Location.StartListening();

            m_Averager = new PositionAverager(m_Location, 10);
            m_Averager.PositionUpdated += Averager_PositionUpdated;
            StartUpdateLocation();
        }

        void Averager_PositionUpdated(GeoPoint position)
        {
            if (position != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    m_lblCount.Text = string.Format("Position Count: {0}", position.CountOfPositions);
                    m_lblLatitude.Text = position.Latitude.AsLatitude();
                    m_lblLongitude.Text = position.Longitude.AsLongitude();
                });
            }
        }

        public async Task<MediaFile> TakePicture()
        {
            var cmso = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Front,
                MaxPixelDimension = 400
            };

            return await m_MediaPicker.TakePhotoAsync(cmso).ContinueWith(t =>
            {
                if (t.IsCompleted
                    && t.Status == TaskStatus.RanToCompletion)
                {
                    var mediaFile = t.Result;

                    return mediaFile;
                }

                return null;
            }, m_Scheduler);
        }

        async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            if (m_MediaPicker.IsCameraAvailable)
            {
                MediaFile file = await TakePicture();

                if (file != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            m_imgHydrant.Source = ImageSource.FromStream(() => file.Source);
                        });
                }
            }
        }

        private void Cleanup()
        {
            m_Averager.PositionUpdated -= Averager_PositionUpdated;
            m_Averager = null;
            m_Location.StopListening();
            m_Location = null;
        }

        void CancelButton_Clicked(object sender, System.EventArgs e)
        {
            Cleanup();
            Navigation.PopModalAsync(true);
        }

        void SaveButton_Clicked(object sender, EventArgs e)
        {


            Cleanup();
            Navigation.PopModalAsync(true);
        }

        private void StartUpdateLocation()
        {
            Task t = Task.Factory.StartNew(() => m_Averager.Start());
        }
    }
}
