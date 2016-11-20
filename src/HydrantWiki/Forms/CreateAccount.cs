using System;
using System.Threading.Tasks;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.ResponseObjects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class CreateAccount : AbstractPage
    {
        HWHeader m_Header;
        HWButton m_btnCancel;

        StackLayout InsideLayout;

        HWLabel lblPickUsername;
        HWTextEntry txtUsername;

        HWLabel lblEnterEmail;
        HWTextEntry txtEmail;

        HWLabel lblPassword1;
        HWTextEntry txtPassword1;

        HWLabel lblPassword2;
        HWTextEntry txtPassword2;

        HWFormButton btnCreateAccount;

        bool usernameAvailable;
        bool emailAvailable;
        bool passwordsMatch;

        public CreateAccount() : base(DisplayConstants.FormCreateAccount)
        {
            m_Header = new HWHeader(DisplayConstants.FormCreateAccount);
            OutsideLayout.Children.Add(m_Header);
            usernameAvailable = false;
            emailAvailable = false;
            passwordsMatch = false;

            m_btnCancel = new HWButton
            {
                Text = DisplayConstants.Cancel,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White
            };
            m_btnCancel.Clicked += Cancel_Clicked;
            m_Header.SetLeftButton(m_btnCancel);

            InsideLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Margin = new Thickness(5, 5, 5, 5)
            };
            OutsideLayout.Children.Add(InsideLayout);

            lblPickUsername = new HWLabel
            {
                Text = DisplayConstants.PickUsername,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            InsideLayout.Children.Add(lblPickUsername);

            txtUsername = new HWTextEntry
            {
                Title = DisplayConstants.Username
            };
            txtUsername.TextChanged += Username_TextChanged;
            InsideLayout.Children.Add(txtUsername);

            lblEnterEmail = new HWLabel
            {
                Text = DisplayConstants.EnterEmail,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            InsideLayout.Children.Add(lblEnterEmail);

            txtEmail = new HWTextEntry
            {
                Title = DisplayConstants.Email
            };
            txtEmail.TextChanged += Email_TextChanged;
            InsideLayout.Children.Add(txtEmail);

            lblPassword1 = new HWLabel
            {
                Text = DisplayConstants.EnterPassword
            };
            InsideLayout.Children.Add(lblPassword1);

            txtPassword1 = new HWTextEntry
            {
                Title = DisplayConstants.Password,
                IsPassword = true
            };
            txtPassword1.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword1);

            lblPassword2 = new HWLabel
            {
                Text = DisplayConstants.VerifyPassword
            };
            InsideLayout.Children.Add(lblPassword2);

            txtPassword2 = new HWTextEntry
            {
                Title = DisplayConstants.Password,
                IsPassword = true
            };
            txtPassword2.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword2);

            btnCreateAccount = new HWFormButton
            {
                Text = DisplayConstants.CreateAccount,
                IsEnabled = false,
                Margin = new Thickness(0, 20, 0, 0)
            };
            btnCreateAccount.Clicked += CreateAccount_Clicked;
            InsideLayout.Children.Add(btnCreateAccount);
        }

        void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            var taskCheckUsername = Task.Factory.StartNew(() => CheckUsername());
        }

        void CheckUsername()
        {
            string username = txtUsername.Text;
            string message;
            usernameAvailable = false;

            if (username.Length == 0)
            {
                message = DisplayConstants.PickUsername;
            } else if (username.Length < 6)
            {
                message = DisplayConstants.PickUsername + " " + DisplayConstants.TooShort;
            } else {
                HWManager manager = HWManager.GetInstance();

                try
                {
                    bool available = manager.ApiManager.UsernameAvailable(username);

                    if (available)
                    {
                        usernameAvailable = true;
                        message = DisplayConstants.PickUsername + " " + DisplayConstants.Available;
                    } else {
                        message = DisplayConstants.PickUsername + " " + DisplayConstants.NotAvailable; ;
                    }
                }
                catch (Exception ex)
                {
                    message = DisplayConstants.PickUsername;
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                lblPickUsername.Text = message;
                EnableCreateButton();
            });
        }

        void EnableCreateButton()
        {
            if (usernameAvailable && emailAvailable && passwordsMatch)
            {
                btnCreateAccount.IsEnabled = true;
            } else {
                btnCreateAccount.IsEnabled = false;
            }
        }

        void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            var taskCheckEmail = Task.Factory.StartNew(() => CheckEmail());
        }

        void CheckEmail()
        {
            string email = txtEmail.Text;
            string message;
            emailAvailable = false;

            if (email.Length == 0)
            {
                message = DisplayConstants.EnterEmail;
            } else {
                if (email.Contains("@")
                    && email.Contains("."))
                {
                    HWManager manager = HWManager.GetInstance();

                    bool available = manager.ApiManager.EmailInUse(email);

                    if (available)
                    {
                        emailAvailable = true;
                        message = DisplayConstants.EnterEmail + " " + DisplayConstants.Available;
                    } else {
                        message = DisplayConstants.EnterEmail + " " + DisplayConstants.NotAvailable;
                    }
                } else {
                    message = DisplayConstants.EnterEmail;
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                lblEnterEmail.Text = message;
                EnableCreateButton();
            });
        }

        void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordsMatch = false;
            string message1 = DisplayConstants.EnterPassword;
            string message2 = DisplayConstants.VerifyPassword;

            string pwd1 = txtPassword1.Text;
            string pwd2 = txtPassword2.Text;

            if (!string.IsNullOrEmpty(pwd1)
                && !string.IsNullOrEmpty(pwd2))
            {
                //Both aren't null
                if (pwd1.Length < 8)
                {
                    message1 = DisplayConstants.EnterPassword + " " + DisplayConstants.TooShort;
                } else {
                    if (pwd1.Equals(pwd2))
                    {
                        passwordsMatch = true;
                        message2 = DisplayConstants.EnterPassword + " " + DisplayConstants.Matching;
                    } else {
                        message2 = DisplayConstants.EnterPassword + " " + DisplayConstants.NotMatching;
                    }
                }
            } else {
                if (pwd1.Length < 8)
                {
                    message1 = DisplayConstants.EnterPassword + " " + DisplayConstants.TooShort;
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                if (!string.IsNullOrEmpty(message1))
                {
                    lblPassword1.Text = message1;
                }

                if (!string.IsNullOrEmpty(message2))
                {
                    lblPassword2.Text = message2;
                }

                EnableCreateButton();
            });
        }

        async void CreateAccount_Clicked(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string password1 = txtPassword1.Text;
            string password2 = txtPassword2.Text;

            if (password1 != null
                && password1.Length >= 8
                && password1.Equals(password2)
                && username != null
                && email != null)
            {
                try
                {
                    CreateAccountResponse response = HWManager.GetInstance().ApiManager.CreateAccount(username, email, password1);

                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        await DisplayAlert(
                            DisplayConstants.AppName,
                            response.Message,
                            DisplayConstants.OK);
                    }

                    if (response.Success)
                    {
                        await Navigation.PopModalAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert(
                        DisplayConstants.AppName,
                        DisplayConstants.WebRequestError,
                        DisplayConstants.OK);
                }
            } else {
                await DisplayAlert(
                    DisplayConstants.AppName,
                    DisplayConstants.UserDataNotComplete,
                    DisplayConstants.OK);
            }
        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
    }
}
