using System;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class CreateAccount : AbstractPage
    {
        WebView m_webAbout;
        HWHeader m_Header;
        HWButton m_btnCancel;

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

            string url = HWManager.GetInstance().PlatformManager.ApiHost + "/createaccount";

            m_webAbout = new WebView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Source = url,
                Margin = new Thickness(10, 10, 10, 10)
            };
            OutsideLayout.Children.Add(m_webAbout);
        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
    }
}
