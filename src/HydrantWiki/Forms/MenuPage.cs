using Xamarin.Forms;
using HydrantWiki.Controls;
using HydrantWiki.Constants;

namespace HydrantWiki.Forms
{
    public class MenuPage : ContentPage
    {
        public MenuListView Menu { get; set; }

        public MenuPage()
        {
            //Icon = "settings.png";
            Title = DisplayConstants.Menu;
            BackgroundColor = Color.FromHex(UIConstants.MenuTitleColor);

            Menu = new MenuListView();

            var menuLabel = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new HWLabel
                {
                    TextColor = Color.FromHex(UIConstants.MenuTitleTextColor),
                    Text = DisplayConstants.Menu,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold
                }
            };

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);

            Content = layout;
        }
    }
}

