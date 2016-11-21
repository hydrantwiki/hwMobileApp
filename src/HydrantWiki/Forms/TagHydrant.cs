using System;
using System.Threading.Tasks;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Helpers;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using HydrantWiki.ResponseObjects;
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
        private HWButton CancelButton;
        private HWButton SaveButton;

        private StackLayout m_layoutPhoto;
        private Image m_imgHydrant;
        private HWFormButton m_btnTakePhoto;
        private HWLabel m_lblCount;
        private HWLabel m_lblLatitude;
        private HWLabel m_lblLongitude;

        public TagHydrant()
            : base(DisplayConstants.FormTagHydrant)
        {
            m_MediaPicker = DependencyService.Get<IMediaPicker>();
            var device = Resolver.Resolve<IDevice>();
            m_MediaPicker = m_MediaPicker ?? device.MediaPicker;


            m_Header = new HWHeader(DisplayConstants.FormTagHydrant)
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
            CancelButton.Clicked += CancelButton_Clicked;
            m_Header.SetLeftButton(CancelButton);

            SaveButton = new HWButton
            {
                Text = DisplayConstants.Save,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
            };
            SaveButton.Clicked += SaveButton_Clicked;
            m_Header.SetRightButton(SaveButton);

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
                HasShadow = false,
                Margin = new Thickness(3, 3, 3, 3),
                Padding = new Thickness(0, 0, 0, 0)
            };
            m_layoutPhoto.Children.Add(imageFrame);

            m_imgHydrant = new Image
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFit,
                Margin = new Thickness(0, 0, 0, 0)
            };
            imageFrame.Content = m_imgHydrant;

            m_btnTakePhoto = new HWFormButton
            {
                Text = DisplayConstants.TakePhoto,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 80,
                HeightRequest = 40,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button))
            };
            m_btnTakePhoto.Clicked += TakePhoto_Clicked;
            m_layoutPhoto.Children.Add(m_btnTakePhoto);

            if (m_MediaPicker.IsCameraAvailable)
            {
                m_btnTakePhoto.IsEnabled = true;
                SaveButton.IsEnabled = false;
            } else
            {
                m_btnTakePhoto.IsEnabled = false;
                SaveButton.IsEnabled = true;
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
                Text = DisplayConstants.PositionCount,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            lableLayout.Children.Add(m_lblCount);

            m_lblLatitude = new HWLabel
            {
                Text = DisplayConstants.Latitude,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            lableLayout.Children.Add(m_lblLatitude);

            m_lblLongitude = new HWLabel
            {
                Text = DisplayConstants.Longitude,
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
                    m_lblCount.Text = string.Format(
                        DisplayConstants.PositionCountNumber,
                        position.CountOfPositions);

                    m_lblLatitude.Text = position.Latitude.AsLatitude();
                    m_lblLongitude.Text = position.Longitude.AsLongitude();
                });
            }
        }

        public async Task<MediaFile> TakePicture()
        {
            var cmso = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Rear,
                SaveMediaOnCapture = false
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
                MediaFile mediaFile = await TakePicture();

                if (mediaFile != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        m_imgHydrant.Source = ImageSource.FromStream(() => mediaFile.Source);
                        SaveButton.IsEnabled = true;
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

        void CancelButton_Clicked(object sender, EventArgs e)
        {
            Cleanup();
            Navigation.PopModalAsync(true);
        }

        async void SaveButton_Clicked(object sender, EventArgs e)
        {
            HWManager manager = HWManager.GetInstance();

            Guid? imageGuid = null;
            string filename = null;

            GeoPoint average = m_Averager.Average.GetAverage();

            if (average != null)
            {
                if (m_imgHydrant.Source != null)
                {
                    imageGuid = Guid.NewGuid();

                    string file = string.Format("{0}.jpg", imageGuid);
                    filename = manager.PlatformManager.GetLocalImageFilename(file);
                    await manager.PlatformManager.SaveImage(m_imgHydrant.Source, filename);
                }

                Tag tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    ImageGuid = imageGuid,
                    TagTime = average.DeviceDateTime,
                    Position = average,
                    SentToServer = false
                };

                //Save Tag locally - Figure out
                manager.Persist(tag);

                if (manager.PlatformManager.HasNetworkConnectivity)
                {
                    //Save tag to server if connected
                    TagResponse response = manager.ApiManager.SaveTag(HydrantWikiApp.User, tag);
                    if (response != null)
                    {
                        tag.ThumbnailUrl = response.ThumbnailUrl;
                        tag.ImageUrl = response.ImageUrl;
                    }

                    manager.ApiManager.SaveTagImage(HydrantWikiApp.User, filename);

                    tag.SentToServer = true;
                    manager.Persist(tag);

                    manager.ApiManager.Log(LogLevels.Info,
                                           string.Format("Tag Saved by {0}", HydrantWikiApp.User.Username));
                }
            }

            Cleanup();
            await Navigation.PopModalAsync(true);
        }

        private void StartUpdateLocation()
        {
            Task t = Task.Factory.StartNew(() => m_Averager.Start());
        }
    }
}
