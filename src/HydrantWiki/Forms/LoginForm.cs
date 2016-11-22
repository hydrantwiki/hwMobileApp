using System;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class LoginForm : AbstractPage
    {
        private HWTextEntry txtEmail;
        private HWTextEntry txtPassword;
        private HWHeader m_Header;
        private HWFormButton btnLogin;
        private HWFormButton btnForgotPassword;
        private HWFormButton btnCreateAccount;
        private ContentView spacer;

        public LoginForm() : base(DisplayConstants.Login)
        {
            m_Header = new HWHeader(DisplayConstants.Login);
            OutsideLayout.Children.Add(m_Header);

            txtEmail = new HWTextEntry()
            {
                Title = DisplayConstants.Email,
                Placeholder = DisplayConstants.EmailInstruction
            };
            txtEmail.TextChanged += Email_TextChanged;
            OutsideLayout.Children.Add(txtEmail);

            txtPassword = new HWTextEntry()
            {
                Title = DisplayConstants.Password,
                Placeholder = DisplayConstants.PasswordInstruction,
                IsPassword = true
            };
            OutsideLayout.Children.Add(txtPassword);

            btnLogin = new HWFormButton()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.Login,
                Margin = new Thickness(10, 10, 10, 10),
                IsEnabled = false
            };
            btnLogin.Clicked += btnLoginClicked;
            OutsideLayout.Children.Add(btnLogin);

            spacer = new ContentView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            OutsideLayout.Children.Add(spacer);

            btnForgotPassword = new HWFormButton()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.ForgotPassword,
                Margin = new Thickness(10, 10, 10, 10)
            };
            btnForgotPassword.Clicked += btnForgotPasswordClicked;
            OutsideLayout.Children.Add(btnForgotPassword);

            btnCreateAccount = new HWFormButton()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.CreateAnAccount,
                Margin = new Thickness(10, 10, 10, 10)
            };
            btnCreateAccount.Clicked += btnCreateAccountClicked;
            OutsideLayout.Children.Add(btnCreateAccount);
        }

        void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            string email = txtEmail.Text;

            if (email.Contains("@")
                && email.Contains("."))
            {
                btnLogin.IsEnabled = true;
            }
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
            string email = txtEmail.Text;
            string password = txtPassword.Text;

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
