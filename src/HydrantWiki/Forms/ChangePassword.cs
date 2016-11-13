using System;
using System.Threading.Tasks;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.ResponseObjects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class ChangePassword : AbstractPage
    {
        HWHeader m_Header;
        HWButton m_btnCancel;

        StackLayout InsideLayout;

        HWLabel lblPickUsername;
        HWTextEntry txtUsername;

        HWLabel lblExistingPassword;
        HWTextEntry txtExistingPassword;

        HWLabel lblPassword1;
        HWTextEntry txtPassword1;

        HWLabel lblPassword2;
        HWTextEntry txtPassword2;

        HWButton btnChangePassword;

        bool passwordsMatch;

        public ChangePassword() : base("Change Password")
        {
            m_Header = new HWHeader("Change Password");
            OutsideLayout.Children.Add(m_Header);
            passwordsMatch = false;

            m_btnCancel = new HWButton
            {
                Text = "Cancel",
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

            txtUsername = new HWTextEntry
            {
                Title = "Username",
                Text = HydrantWikiApp.User.Username,
                IsEnabled = false
            };
            InsideLayout.Children.Add(txtUsername);

            lblExistingPassword = new HWLabel
            {
                Text = "Enter your Existing Password",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            InsideLayout.Children.Add(lblExistingPassword);

            txtExistingPassword = new HWTextEntry
            {
                Title = "Password"
            };
            InsideLayout.Children.Add(txtExistingPassword);

            lblPassword1 = new HWLabel
            {
                Text = "Enter New Password"
            };
            InsideLayout.Children.Add(lblPassword1);

            txtPassword1 = new HWTextEntry
            {
                Title = "New Password",
                IsPassword = true
            };
            txtPassword1.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword1);

            lblPassword2 = new HWLabel
            {
                Text = "Verify New Password"
            };
            InsideLayout.Children.Add(lblPassword2);

            txtPassword2 = new HWTextEntry
            {
                Title = "Password",
                IsPassword = true
            };
            txtPassword2.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword2);

            btnChangePassword = new HWButton
            {
                Text = "Change Password",
                BorderColor = Color.Black,
                BorderWidth = 1,
                BackgroundColor = Color.White,
                IsEnabled = false,
                Margin = new Thickness(0, 20, 0, 0)
            };
            btnChangePassword.Clicked += ChangePassword_Clicked;
            InsideLayout.Children.Add(btnChangePassword);
        }

        void EnableChangePasswordButton()
        {
            if (passwordsMatch)
            {
                btnChangePassword.IsEnabled = true;
            } else {
                btnChangePassword.IsEnabled = false;
            }
        }

        void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordsMatch = false;
            string message1 = "Enter Password";
            string message2 = "Verify Password";

            string pwd1 = txtPassword1.Text;
            string pwd2 = txtPassword2.Text;

            if (!string.IsNullOrEmpty(pwd1)
                && !string.IsNullOrEmpty(pwd2))
            {
                //Both aren't null
                if (pwd1.Length < 8)
                {
                    message1 = "Enter Password (Too Short)";
                } else {
                    if (pwd1.Equals(pwd2))
                    {
                        passwordsMatch = true;
                        message2 = "Verify Password (Matching)";
                    } else {
                        message2 = "Verify Password (Not Matching)";
                    }
                }
            } else {
                if (pwd1.Length < 8)
                {
                    message1 = "Enter Password (Too short)";
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

                EnableChangePasswordButton();
            });
        }

        async void ChangePassword_Clicked(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string existingPassword = txtExistingPassword.Text;
            string password1 = txtPassword1.Text;
            string password2 = txtPassword2.Text;

            if (password1 != null
                && password1.Length >= 8
                && password1.Equals(password2)
                && username != null
                && existingPassword != null)
            {
                try
                {
                    CreateAccountResponse response = HWManager.GetInstance().ApiManager.CreateAccount(username, email, password1);

                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        await DisplayAlert("HydrantWiki", response.Message, "Ok");
                    }

                    if (response.Success)
                    {
                        await Navigation.PopModalAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("HydrantWiki", "An error occured processing the request", "Ok");
                }
            } else {
                await DisplayAlert("HydrantWiki", "User data not complete", "Ok");
            }
        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
    }
}
