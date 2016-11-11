using System;
using System.Threading.Tasks;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
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

        HWButton btnCreateAccount;

        bool usernameAvailable;
        bool emailAvailable;
        bool passwordsMatch;

        public CreateAccount() : base("Create Account")
        {
            m_Header = new HWHeader("Create Account");
            OutsideLayout.Children.Add(m_Header);
            usernameAvailable = false;
            emailAvailable = false;
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

            lblPickUsername = new HWLabel
            {
                Text = "Pick your username",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            InsideLayout.Children.Add(lblPickUsername);

            txtUsername = new HWTextEntry
            {
                Title = "Username"
            };
            txtUsername.TextChanged += Username_TextChanged;
            InsideLayout.Children.Add(txtUsername);

            lblEnterEmail = new HWLabel
            {
                Text = "Enter your email",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            InsideLayout.Children.Add(lblEnterEmail);

            txtEmail = new HWTextEntry
            {
                Title = "Email"
            };
            txtEmail.TextChanged += Email_TextChanged;
            InsideLayout.Children.Add(txtEmail);

            lblPassword1 = new HWLabel
            {
                Text = "Enter Password"
            };
            InsideLayout.Children.Add(lblPassword1);

            txtPassword1 = new HWTextEntry
            {
                Title = "Password",
                IsPassword = true
            };
            txtPassword1.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword1);

            lblPassword2 = new HWLabel
            {
                Text = "Verify Password"
            };
            InsideLayout.Children.Add(lblPassword2);

            txtPassword2 = new HWTextEntry
            {
                Title = "Password",
                IsPassword = true
            };
            txtPassword2.TextChanged += Password_TextChanged;
            InsideLayout.Children.Add(txtPassword2);

            btnCreateAccount = new HWButton
            {
                Text = "Create Account",
                BorderColor = Color.Black,
                BorderWidth = 1,
                BackgroundColor = Color.White,
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
                message = "Enter your email";
            } else if (username.Length < 6)
            {
                message = "Pick your username (Too short)";
            } else {
                HWManager manager = HWManager.GetInstance();

                bool available = manager.ApiManager.UsernameAvailable(username);

                if (available)
                {
                    usernameAvailable = true;
                    message = "Pick your username (Available)";
                } else {
                    message = "Pick your username (Not Available)";
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
                message = "Enter your email";
            } else {
                HWManager manager = HWManager.GetInstance();

                bool available = manager.ApiManager.EmailInUse(email);

                if (available)
                {
                    emailAvailable = true;
                    message = "Enter your email (Available)";
                } else {
                    message = "Enter your email (Not Available)";
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

                EnableCreateButton();
            });
        }

        void CreateAccount_Clicked(object sender, EventArgs e)
        {

        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
    }
}
