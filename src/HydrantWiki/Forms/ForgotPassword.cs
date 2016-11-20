using System;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class ForgotPassword : AbstractPage
    {
        HWHeader m_Header;
        HWButton m_btnCancel;

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
        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
    }
}
