using Xamarin.Forms;
using System.Collections.Generic;
using HydrantWiki.Constants;
using HydrantWiki.Cells;
using HydrantWiki.Data;
using HydrantWiki.Objects;

namespace HydrantWiki.Controls
{
    public class MenuListView : AbstractListView
    {
        public MenuListView()
        {
            BackgroundColor = Color.FromHex(UIConstants.MenuPageListBackgroundColor);

            ItemTemplate = new DataTemplate(typeof(MenuCell));

            List<MenuOption> data = MenuListData.GetMenu();
            ItemsSource = data;

            RowHeight = 60;
        }
    }
}

