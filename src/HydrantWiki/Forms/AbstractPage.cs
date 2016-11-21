using HydrantWiki.Constants;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public abstract class AbstractPage : ContentPage
    {
        protected StackLayout OutsideLayout;

        protected AbstractPage(string title,
                               int? padding = null,
                               int? spacing = null)
        {
            Title = title;
            BackgroundColor = Color.FromHex(UIConstants.AbstractFormBackground);

            if (Device.Idiom == TargetIdiom.Phone)
            {
                if (padding == null)
                {
                    padding = 0;
                }

                if (spacing == null)
                {
                    spacing = 1;
                }
            } else {
                if (padding == null)
                {
                    padding = 0;
                }

                if (spacing == null)
                {
                    spacing = 5;
                }
            }

            OutsideLayout = new StackLayout
            {
                Spacing = spacing.Value,
                VerticalOptions = LayoutOptions.Fill,
                Padding = padding.Value
            };

            Content = OutsideLayout;
        }
    }
}
