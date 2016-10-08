using System;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class SettingsForm : AbstractPage
    {
        private HWButton Logout;

        public SettingsForm() : base("Settings")
        {
            Logout = new HWButton
            {
                Text = "Logout",
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black
            };
            Logout.Clicked += Logout_Clicked;
            OutsideLayout.Children.Add(Logout);
        }

        void Logout_Clicked(object sender, EventArgs e)
        {
            HWManager.GetInstance().SettingManager.ClearUser();

            LoginForm login = new LoginForm();
            Navigation.PushModalAsync(login);
        }
    }
}
