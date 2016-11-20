using System;
using HydrantWiki.Constants;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class HWFormButton : HWButton
    {
        public HWFormButton()
        {
            BackgroundColor = Color.FromHex(UIConstants.FormButtonBackgroundColor);
            TextColor = Color.FromHex(UIConstants.FormButtonTextColor);
            BorderColor = Color.FromHex(UIConstants.FormButtonBorderColor);
            BorderRadius = 0;
            BorderWidth = 1;
            FontFamily = UIConstants.FontFamily;
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            FontAttributes = FontAttributes.Bold;

        }
    }
}
