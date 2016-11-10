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

        public CreateAccount() : base("Create Account")
        {
            m_Header = new HWHeader("Create Account");
            OutsideLayout.Children.Add(m_Header);

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
        }

        void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            var taskCheckUsername = Task.Factory.StartNew(() => CheckUsername());
        }

        void CheckUsername()
        {
            string username = txtUsername.Text;
            string message;

            if (username.Length == 0)
            {
                message = "Pick your username";
            } else if (username.Length < 6)
            {
                message = "Pick your username (Too short)";
            } else {
                HWManager manager = HWManager.GetInstance();

                bool available = manager.ApiManager.UsernameAvailable(txtUsername.Text);

                if (available)
                {
                    message = "Pick your username (Available)";
                } else {
                    message = "Pick your username (Not Available)";
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                lblPickUsername.Text = message;
            });
        }

        void Email_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
    }
}
