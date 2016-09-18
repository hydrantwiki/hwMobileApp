using HydrantWiki.Constants;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class HWLabel : Label
    {
        public HWLabel()
        {
            FontFamily = UIConstants.FontFamily;
            LineBreakMode = LineBreakMode.WordWrap;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            } else {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            }
        }
    }
}

