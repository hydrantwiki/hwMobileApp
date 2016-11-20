using System;
using System.Threading.Tasks;
using HydrantWiki.Constants;
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

        HWLabel lblExistingPassword;
        HWTextEntry txtExistingPassword;

        HWLabel lblPassword1;
        HWTextEntry txtPassword1;

        HWLabel lblPassword2;
        HWTextEntry txtPassword2;

        HWFormButton btnChangePassword;

        bool passwordsMatch;

        public ChangePassword() : base(DisplayConstants.FormChangePassword)
        {
            m_Header = new HWHeader(DisplayConstants.FormChangePassword);
            OutsideLayout.Children.Add(m_Header);
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

            lblExistingPassword = new HWLabel
            {
                Text = DisplayConstants.EnterExistingPassword,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            InsideLayout.Children.Add(lblExistingPassword);

            txtExistingPassword = new HWTextEntry
            {
                Title = DisplayConstants.Existing,
                IsPassword = true
            };
            InsideLayout.Children.Add(txtExistingPassword);

            lblPassword1 = new HWLabel
            {
                Text = DisplayConstants.EnterNewPassword
            };
            InsideLayout.Children.Add(lblPassword1);

            txtPassword1 = new HWTextEntry
            {
                Title = DisplayConstants.New,
                IsPassword = true
            };
            txtPassword1.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword1);

            lblPassword2 = new HWLabel
            {
                Text = DisplayConstants.VerifyNewPassword
            };
            InsideLayout.Children.Add(lblPassword2);

            txtPassword2 = new HWTextEntry
            {
                Title = DisplayConstants.Verify,
                IsPassword = true
            };
            txtPassword2.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword2);

            btnChangePassword = new HWFormButton
            {
                Text = DisplayConstants.ChangePassword,
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
            string message1 = DisplayConstants.EnterNewPassword;
            string message2 = DisplayConstants.VerifyNewPassword;

            string pwd1 = txtPassword1.Text;
            string pwd2 = txtPassword2.Text;

            if (!string.IsNullOrEmpty(pwd1)
                && !string.IsNullOrEmpty(pwd2))
            {
                //Both aren't null
                if (pwd1.Length < 8)
                {
                    message1 = DisplayConstants.EnterNewPassword + " " + DisplayConstants.TooShort;
                } else {
                    if (pwd1.Equals(pwd2))
                    {
                        passwordsMatch = true;
                        message2 = DisplayConstants.VerifyNewPassword + " " + DisplayConstants.Matching;
                    } else {
                        message2 = DisplayConstants.VerifyNewPassword + " " + DisplayConstants.NotMatching;
                    }
                }
            } else {
                if (pwd1.Length < 8)
                {
                    message1 = DisplayConstants.EnterNewPassword + " " + DisplayConstants.TooShort;
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
            string existingPassword = txtExistingPassword.Text;
            string password1 = txtPassword1.Text;
            string password2 = txtPassword2.Text;

            if (password1 != null
                && password1.Length >= 8
                && password1.Equals(password2)
                && existingPassword != null)
            {
                try
                {
                    ChangePasswordResponse response =
                        HWManager.GetInstance().ApiManager.ChangePassword(HydrantWikiApp.User, existingPassword, password1);

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
