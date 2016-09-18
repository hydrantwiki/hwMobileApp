using System;
using System.Collections.Generic;
using HydrantWiki.Forms;
using HydrantWiki.Objects;

namespace HydrantWiki.Data
{
    public static class MenuListData
    {
        public static List<MenuOption> GetMenu()
        {
            List<MenuOption> menuItems = new List<MenuOption>();

            menuItems.Add(new MenuOption()
            {
                Title = "Home",
                TargetType = typeof(DefaultForm)
            });


            return menuItems;
        }
    }
}

