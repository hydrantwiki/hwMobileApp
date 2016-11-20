using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using HydrantWiki.ResponseObjects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class SettingsForm : AbstractPage
    {
        private HWButton btnLogout;
        private HWButton btnChangePassword;
        private HWLabel lblUnsynced;
        private HWButton btnSync;

        private StackLayout m_InsideLayout;

        public SettingsForm() : base(DisplayConstants.FormSettings)
        {
            m_InsideLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(10, 10, 10, 10),
            };
            OutsideLayout.Children.Add(m_InsideLayout);

            lblUnsynced = new HWLabel
            {
                Text = DisplayConstants.TagsUnsent,
                HorizontalTextAlignment = TextAlignment.Center
            };
            m_InsideLayout.Children.Add(lblUnsynced);

            btnSync = new HWButton
            {
                Text = DisplayConstants.SyncTags,
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black,
                IsEnabled = false
            };
            btnSync.Clicked += Sync_Clicked;
            m_InsideLayout.Children.Add(btnSync);

            btnChangePassword = new HWButton
            {
                Text = DisplayConstants.ChangePassword,
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black,
                Margin = new Thickness(0, 10, 0, 0)
            };
            btnChangePassword.Clicked += ChangePassword_Clicked;
            m_InsideLayout.Children.Add(btnChangePassword);

            btnLogout = new HWButton
            {
                Text = DisplayConstants.Logout,
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black,
                Margin = new Thickness(0, 10, 0, 0),
                VerticalOptions = LayoutOptions.End
            };
            btnLogout.Clicked += Logout_Clicked;
            m_InsideLayout.Children.Add(btnLogout);

            if (HWManager.GetInstance().PlatformManager.HasNetworkConnectivity)
            {
                btnSync.Text = DisplayConstants.SyncTags;
                btnSync.IsEnabled = true;
            } else
            {
                btnSync.Text = DisplayConstants.SyncTagsNoNetwork;
                btnSync.IsEnabled = false;
            }

            Task t = Task.Factory.StartNew(() => LoadTagCount());
        }

        void LoadTagCount()
        {
            List<Tag> tags = HWManager.GetInstance().GetTagsNotSentToServer();

            if (tags != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblUnsynced.Text = string.Format(DisplayConstants.TagsUnsentNumber, tags.Count);
                    if (tags.Count > 0)
                    {
                        btnSync.IsEnabled = true;
                    } else {
                        btnSync.IsEnabled = false;
                    }
                });
            }
        }

        void ChangePassword_Clicked(object sender, EventArgs e)
        {
            ChangePassword cp = new ChangePassword();
            Navigation.PushModalAsync(cp);
        }

        void Sync_Clicked(object sender, EventArgs e)
        {
            HWManager manager = HWManager.GetInstance();
            List<Tag> tagsNotSent = manager.GetTagsNotSentToServer();

            if (tagsNotSent != null)
            {
                foreach (var tag in tagsNotSent)
                {
                    try
                    {
                        string file = string.Format("{0}.jpg", tag.ImageGuid);
                        string filename = manager.PlatformManager.GetLocalImageFilename(file);

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
                    }
                    catch (Exception ex)
                    {
                        //TODO - Log Error
                    }
                }
            }

            LoadTagCount();
        }

        void Logout_Clicked(object sender, EventArgs e)
        {
            HWManager.GetInstance().SettingManager.ClearUser();

            LoginForm login = new LoginForm();
            Navigation.PushModalAsync(login);
        }
    }
}
