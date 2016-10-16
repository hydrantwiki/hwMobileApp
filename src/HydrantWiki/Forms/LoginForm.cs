using System;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class LoginForm : AbstractPage
    {
        private HWTextEntry m_txtUsername;
        private HWTextEntry m_txtPassword;
        private HWHeader m_Header;
        private HWButton m_btnLogin;

        public LoginForm() : base(DisplayConstants.Login)
        {
            m_Header = new HWHeader(DisplayConstants.Login);
            OutsideLayout.Children.Add(m_Header);

            m_txtUsername = new HWTextEntry()
            {
                Title = DisplayConstants.Username,
                Placeholder = DisplayConstants.UsernameInstruction
            };
            OutsideLayout.Children.Add(m_txtUsername);

            m_txtPassword = new HWTextEntry()
            {
                Title = DisplayConstants.Password,
                Placeholder = DisplayConstants.PasswordInstruction,
                IsPassword = true
            };
            OutsideLayout.Children.Add(m_txtPassword);

            m_btnLogin = new HWButton()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.Login,
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black,
                Margin = new Thickness(10, 10, 10, 10)
            };
            m_btnLogin.Clicked += btnLoginClicked;
            OutsideLayout.Children.Add(m_btnLogin);
        }

        void btnLoginClicked(object sender, EventArgs e)
        {
            string username = m_txtUsername.Text;
            string password = m_txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(password))
            {
                DisplayAlert(
                    DisplayConstants.AppName,
                    DisplayConstants.UsernameAndPasswordRequired,
                    DisplayConstants.OK);
            } else {
                bool result = HWManager.GetInstance().Login(username, password);

                if (result)
                {
                    Navigation.PopModalAsync(true);
                } else {
                    DisplayAlert(
                        DisplayConstants.AppName,
                        DisplayConstants.InvalidUsernameOrPasssword,
                        DisplayConstants.OK);
                }
            }
        }
    }
}
