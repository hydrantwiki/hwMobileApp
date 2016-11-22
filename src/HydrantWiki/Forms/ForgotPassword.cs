using System;
using System.Threading.Tasks;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class ForgotPassword : AbstractPage
    {
        private StackLayout InsideLayout;
        private HWHeader m_Header;
        private HWButton m_btnCancel;
        private HWLabel lblEnterEmail;
        private HWTextEntry txtEmail;
        private HWFormButton btnRequestReset;
        private HWLabel lblInstructions;
        private HWTextEntry txtCode;
        private HWLabel lblPassword1;
        private HWTextEntry txtPassword1;
        private HWLabel lblPassword2;
        private HWTextEntry txtPassword2;
        private HWFormButton btnSetPassword;

        private bool emailEntered = false;
        private bool passwordsMatch = false;
        private bool codeEntered = false;

        public ForgotPassword() : base(DisplayConstants.FormForgotPassword)
        {
            m_Header = new HWHeader(DisplayConstants.FormForgotPassword);
            OutsideLayout.Children.Add(m_Header);

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

            btnRequestReset = new HWFormButton
            {
                Text = DisplayConstants.ResetPassword,
                IsEnabled = false
            };
            btnRequestReset.Clicked += RequestReset_Clicked;
            InsideLayout.Children.Add(btnRequestReset);

            lblInstructions = new HWLabel
            {
                Text = DisplayConstants.CheckEmail,
                IsVisible = false
            };
            InsideLayout.Children.Add(lblInstructions);

            txtCode = new HWTextEntry
            {
                Title = DisplayConstants.Code,
                IsVisible = false
            };
            txtCode.TextChanged += Code_TextChanged;
            InsideLayout.Children.Add(txtCode);

            lblPassword1 = new HWLabel
            {
                Text = DisplayConstants.EnterPassword,
                IsVisible = false
            };
            InsideLayout.Children.Add(lblPassword1);

            txtPassword1 = new HWTextEntry
            {
                Title = DisplayConstants.Password,
                IsPassword = true,
                IsVisible = false
            };
            txtPassword1.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword1);

            lblPassword2 = new HWLabel
            {
                Text = DisplayConstants.VerifyPassword,
                IsVisible = false
            };
            InsideLayout.Children.Add(lblPassword2);

            txtPassword2 = new HWTextEntry
            {
                Title = DisplayConstants.Password,
                IsPassword = true,
                IsVisible = false
            };
            txtPassword2.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword2);

            btnSetPassword = new HWFormButton
            {
                Text = DisplayConstants.SetPassword,
                IsVisible = false,
                IsEnabled = false,
                Margin = new Thickness(0, 20, 0, 0),
            };
            btnSetPassword.Clicked += SetPassword_Clicked;
            InsideLayout.Children.Add(btnSetPassword);
        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }

        void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            Task.Factory.StartNew(() => CheckEmail());
        }

        void EnableCreateButton()
        {
            if (passwordsMatch && codeEntered)
            {
                btnSetPassword.IsEnabled = true;
            } else {
                btnSetPassword.IsEnabled = false;
            }
        }

        void Code_TextChanged(object sender, TextChangedEventArgs e)
        {
            string code = txtCode.Text;

            if (code.Length < 8)
            {
                codeEntered = false;
            } else {
                codeEntered = true;
            }

            EnableCreateButton();
        }

        void CheckEmail()
        {
            string email = txtEmail.Text;

            if (email.Contains("@")
                && email.Contains("."))
            {
                emailEntered = true;
            }

            EnablePhase2();
        }


        void RequestReset_Clicked(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => RequestReset());
        }

        void RequestReset()
        {
            HWManager manager = HWManager.GetInstance();

            bool result = manager.ApiManager.RequestPasswordReset(txtEmail.Text);

            if (result)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert(DisplayConstants.AppName,
                                 DisplayConstants.CheckEmail,
                                 DisplayConstants.OK);
                });
            }
        }

        private void EnablePhase2()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (emailEntered)
                {
                    btnRequestReset.IsEnabled = true;
                    lblInstructions.IsVisible = true;
                    txtCode.IsVisible = true;
                    lblPassword1.IsVisible = true;
                    txtPassword1.IsVisible = true;
                    lblPassword2.IsVisible = true;
                    txtPassword2.IsVisible = true;
                    btnSetPassword.IsVisible = true;
                } else
                {
                    btnRequestReset.IsEnabled = false;
                    lblInstructions.IsVisible = false;
                    txtCode.IsVisible = false;
                    lblPassword1.IsVisible = false;
                    txtPassword1.IsVisible = false;
                    lblPassword2.IsVisible = false;
                    txtPassword2.IsVisible = false;
                    btnSetPassword.IsVisible = false;
                }
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

        async void SetPassword_Clicked(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string code = txtCode.Text;
            string password1 = txtPassword1.Text;
            string password2 = txtPassword2.Text;

            HWManager manager = HWManager.GetInstance();

            if (password1 != null
                && password1.Length >= 8
                && password1.Equals(password2)
                && email != null)
            {
                try
                {
                    var response = manager.ApiManager.ResetPassword(email, code, password1);

                    if (response.Success)
                    {
                        await Navigation.PopModalAsync(true);
                    } else
                    {
                        await DisplayAlert(
                            DisplayConstants.AppName,
                            response.Message,
                            DisplayConstants.OK);
                    }

                }
                catch (Exception ex)
                {
                    manager.ApiManager.Log(LogLevels.Exception, ex.ToString());

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
    }
}
