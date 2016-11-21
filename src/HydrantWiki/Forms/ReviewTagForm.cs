using System;
using HydrantWiki.Cells;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using HydrantWiki.ResponseObjects;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HydrantWiki.Forms
{
    public class ReviewTagForm : AbstractPage
    {
        private TagToReview m_Tag;
        private HWHeader m_Header;
        private HWButton m_Cancel;

        private HWLabel m_User;

        private HWFormButton m_Approve;
        private HWFormButton m_Reject;

        private ReviewTagMap m_Map;
        private Image m_Image;


        private HWLabel m_Nearby;
        private ReviewTagHydrantsListView m_Hydrants;


        public ReviewTagForm() : base(DisplayConstants.FormReviewTag)
        {
            m_Header = new HWHeader(DisplayConstants.FormReviewTag);
            OutsideLayout.Children.Add(m_Header);

            m_Cancel = new HWButton
            {
                Text = DisplayConstants.Cancel,
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

            int left1 = (HydrantWikiApp.ScreenWidth - 200) / 4;

            m_Reject = new HWFormButton
            {
                Text = DisplayConstants.Reject,
                WidthRequest = 100,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            m_Reject.Clicked += RejectClicked;
            AbsoluteLayout.SetLayoutBounds(m_Reject, new Rectangle(left1, 10, 100, 30));
            layout.Children.Add(m_Reject);

            m_Approve = new HWFormButton
            {
                Text = DisplayConstants.Approve,
                WidthRequest = 100,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            m_Approve.Clicked += ApproveClicked;
            AbsoluteLayout.SetLayoutBounds(m_Approve, new Rectangle(3 * left1 + 100, 10, 100, 30));
            layout.Children.Add(m_Approve);

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

            //110 is the fixed item heights 
            int middleHeight = (HydrantWikiApp.ScreenHeight - 110) * 3 / 5;
            int bottomHeight = (HydrantWikiApp.ScreenHeight - 110) * 2 / 5;

            //Add Map
            m_Map = new ReviewTagMap
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            int width = HydrantWikiApp.ScreenWidth / 2 - 3;

            Frame mapFrame = new Frame
            {
                HasShadow = false,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Padding = new Thickness(1, 1, 1, 1),
                Content = m_Map,
                HeightRequest = middleHeight
            };
            AbsoluteLayout.SetLayoutBounds(mapFrame, new Rectangle(2, 80, width, middleHeight));
            layout.Children.Add(mapFrame);

            //Add image
            m_Image = new Image
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            Frame imageFrame = new Frame
            {
                HasShadow = false,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Content = m_Image,
                Padding = new Thickness(1, 1, 1, 1),
                HeightRequest = middleHeight
            };
            AbsoluteLayout.SetLayoutBounds(imageFrame, new Rectangle(4 + width, 80, width, middleHeight));
            layout.Children.Add(imageFrame);

            //Add nearby hydrants
            m_Nearby = new HWLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = DisplayConstants.FormNearbyHydrants,
                Margin = new Thickness(10, 5, 10, 10)
            };
            AbsoluteLayout.SetLayoutBounds(m_Nearby, new Rectangle(0, 80 + middleHeight, AbsoluteLayout.AutoSize, 30));
            layout.Children.Add(m_Nearby);

            m_Hydrants = new ReviewTagHydrantsListView
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                HeightRequest = 100,
            };
            AbsoluteLayout.SetLayoutBounds(m_Hydrants, new Rectangle(0, 80 + middleHeight + 30, AbsoluteLayout.AutoSize, bottomHeight));
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
            if (m_Tag.ImageGuid != null
                && m_Tag.ImageUrl != null)
            {
                m_Image.Source = ImageSource.FromUri(new Uri(m_Tag.ImageUrl));
            }
            m_Hydrants.ItemsSource = _tag.NearbyHydrants;

            foreach (var hydrant in _tag.NearbyHydrants)
            {
                if (hydrant.Position != null)
                {
                    HydrantPin pin = new HydrantPin()
                    {
                        Hydrant = hydrant,
                        Pin = new Pin
                        {
                            Type = PinType.Place,
                            Label = DisplayConstants.Hydrant,
                            Position = new Position(hydrant.Position.Latitude, hydrant.Position.Longitude)
                        }
                    };

                    m_Map.Pins.Add(pin.Pin);
                    m_Map.NearbyHydrants.Add(pin);
                }
            }


            if (m_Tag.Position != null)
            {
                var pos = new Position(m_Tag.Position.Latitude, m_Tag.Position.Longitude);
                var span = MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(.1));

                Pin pin = new Pin()
                {
                    Type = PinType.Generic,
                    Label = DisplayConstants.NewTag,
                    Position = new Position(m_Tag.Position.Latitude, m_Tag.Position.Longitude)
                };

                TagPin tagPin = new TagPin
                {
                    Pin = pin,
                    Tag = m_Tag
                };

                m_Map.TagToReview = tagPin;
                m_Map.Pins.Add(pin);
                m_Map.MoveToRegion(span);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ReviewTagHydrantCell.HydrantMatchClicked += HydrantMatchClicked;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ReviewTagHydrantCell.HydrantMatchClicked -= HydrantMatchClicked;
        }

        void HydrantMatchClicked(Guid hydrantGuid)
        {
            MatchTagResponse response = HWManager.GetInstance().ApiManager.MatchTag(
                HydrantWikiApp.User,
                m_Tag.TagId,
                hydrantGuid);

            if (response.Success)
            {
                Navigation.PopModalAsync(true);
            } else {
                DisplayAlert(
                    DisplayConstants.AppName,
                    string.Format("An error occurred - {0}", response.Message),
                    DisplayConstants.OK);
            }
        }

        void RejectClicked(object sender, EventArgs e)
        {
            HWManager manager = HWManager.GetInstance();

            RejectTagResponse response = manager.ApiManager.RejectTag(
                HydrantWikiApp.User,
                m_Tag.TagId);

            if (response.Success)
            {
                manager.ApiManager.Log(LogLevels.Info,
                                       string.Format("Tag Rejected by {0}", HydrantWikiApp.User.Username));

                Navigation.PopModalAsync(true);
            } else {
                DisplayAlert(
                    DisplayConstants.AppName,
                    string.Format("An error occurred - {0}", response.Message),
                    DisplayConstants.OK);
            }
        }

        void ApproveClicked(object sender, EventArgs e)
        {
            HWManager manager = HWManager.GetInstance();

            ApproveTagResponse response = manager.ApiManager.ApproveTag(
                HydrantWikiApp.User,
                m_Tag.TagId);

            if (response.Success)
            {
                manager.ApiManager.Log(LogLevels.Info,
                                       string.Format("Tag Approved by {0}", HydrantWikiApp.User.Username));

                Navigation.PopModalAsync(true);
            } else {
                DisplayAlert(
                    DisplayConstants.AppName,
                    string.Format("An error occurred - {0}", response.Message),
                    DisplayConstants.OK);
            }
        }
    }
}
