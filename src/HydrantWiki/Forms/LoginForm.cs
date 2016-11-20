using System;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class LoginForm : AbstractPage
    {
        private HWTextEntry m_txtEmail;
        private HWTextEntry m_txtPassword;
        private HWHeader m_Header;
        private HWFormButton m_btnLogin;
        private HWFormButton m_btnForgotPassword;
        private HWFormButton m_btnCreateAccount;
        private ContentView spacer;

        public LoginForm() : base(DisplayConstants.Login)
        {
            m_Header = new HWHeader(DisplayConstants.Login);
            OutsideLayout.Children.Add(m_Header);

            m_txtEmail = new HWTextEntry()
            {
                Title = DisplayConstants.Email,
                Placeholder = DisplayConstants.EmailInstruction
            };
            OutsideLayout.Children.Add(m_txtEmail);

            m_txtPassword = new HWTextEntry()
            {
                Title = DisplayConstants.Password,
                Placeholder = DisplayConstants.PasswordInstruction,
                IsPassword = true
            };
            OutsideLayout.Children.Add(m_txtPassword);

            m_btnLogin = new HWFormButton()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.Login,
                Margin = new Thickness(10, 10, 10, 10)
            };
            m_btnLogin.Clicked += btnLoginClicked;
            OutsideLayout.Children.Add(m_btnLogin);

            spacer = new ContentView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            OutsideLayout.Children.Add(spacer);

            m_btnForgotPassword = new HWFormButton()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.ForgotPassword,
                Margin = new Thickness(10, 10, 10, 10)
            };
            m_btnForgotPassword.Clicked += btnForgotPasswordClicked;
            OutsideLayout.Children.Add(m_btnForgotPassword);

            m_btnCreateAccount = new HWFormButton()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.CreateAnAccount,
                Margin = new Thickness(10, 10, 10, 10)
            };
            m_btnCreateAccount.Clicked += btnCreateAccountClicked;
            OutsideLayout.Children.Add(m_btnCreateAccount);
        }

        void btnCreateAccountClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CreateAccount());
        }

        void btnForgotPasswordClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ForgotPassword());
        }

        void btnLoginClicked(object sender, EventArgs e)
        {
            string email = m_txtEmail.Text;
            string password = m_txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(password))
            {
                DisplayAlert(
                    DisplayConstants.AppName,
                    DisplayConstants.UsernameAndPasswordRequired,
                    DisplayConstants.OK);
            } else {
                bool result = HWManager.GetInstance().Login(email, password);

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
