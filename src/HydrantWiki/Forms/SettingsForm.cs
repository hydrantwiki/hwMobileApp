using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class SettingsForm : AbstractPage
    {
        private HWButton btnLogout;
        private HWLabel lblUnsynced;
        private HWButton btnSync;

        private StackLayout m_InsideLayout;

        public SettingsForm() : base("Settings")
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
                Text = "Tags unsent: ?",
                HorizontalTextAlignment = TextAlignment.Center
            };
            m_InsideLayout.Children.Add(lblUnsynced);

            btnSync = new HWButton
            {
                Text = "Sync Tags",
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black,
                IsEnabled = false
            };
            btnSync.Clicked += Sync_Clicked;
            m_InsideLayout.Children.Add(btnSync);

            btnLogout = new HWButton
            {
                Text = "Logout",
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black,
                Margin = new Thickness(0, 10, 0, 0)
            };
            btnLogout.Clicked += Logout_Clicked;
            m_InsideLayout.Children.Add(btnLogout);

            Task t = Task.Factory.StartNew(() => LoadTagCount());
        }

        void LoadTagCount()
        {
            List<Tag> tags = HWManager.GetInstance().GetTagsNotSentToServer();

            if (tags != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblUnsynced.Text = string.Format("Tags unsent: {0}", tags.Count);
                    if (tags.Count > 0)
                    {
                        btnSync.IsEnabled = true;
                    } else {
                        btnSync.IsEnabled = false;
                    }
                });
            }
        }

        void Sync_Clicked(object sender, EventArgs e)
        {

        }

        void Logout_Clicked(object sender, EventArgs e)
        {
            HWManager.GetInstance().SettingManager.ClearUser();

            LoginForm login = new LoginForm();
            Navigation.PushModalAsync(login);
        }
    }
}
