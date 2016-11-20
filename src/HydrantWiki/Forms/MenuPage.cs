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
            BackgroundColor = Color.FromHex(UIConstants.MenuPageTitleBackgroundColor);

            HWHeader header = new HWHeader("Menu");

            Menu = new MenuListView();

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(header);
            layout.Children.Add(Menu);


            Content = layout;
        }
    }
}

