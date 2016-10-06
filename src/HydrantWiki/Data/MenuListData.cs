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

            menuItems.Add(new MenuOption()
            {
                Title = "Hydrant Map",
                TargetType = typeof(HydrantMap)
            });

            menuItems.Add(new MenuOption()
            {
                Title = "Nearby Hydrants",
                TargetType = typeof(NearbyHydrants)
            });

            menuItems.Add(new MenuOption()
            {
                Title = "About",
                TargetType = typeof(About)
            });

            return menuItems;
        }
    }
}

