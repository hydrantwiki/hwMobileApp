using System;
using HydrantWiki.Controls;

namespace HydrantWiki.Forms
{
    public class LoginForm : AbstractPage
    {
        private HWTextEntry txtUsername;
        private HWTextEntry txtPassword;

        public LoginForm() : base("Login")
        {
            txtUsername = new HWTextEntry()
            {
                Title = "Username",
                Placeholder = "Enter username"
            };
            OutsideLayout.Children.Add(txtUsername);

            txtPassword = new HWTextEntry()
            {
                Title = "Password",
                Placeholder = "Enter password"
            };
            OutsideLayout.Children.Add(txtPassword);
        }
    }
}
