using System;
using System.Collections.Generic;
using HydrantWiki.Forms;
using HydrantWiki.Managers;
using HydrantWiki.Objects;

namespace HydrantWiki.Data
{
    public static class MenuListData
    {
        public static List<MenuOption> GetMenu()
        {
            List<MenuOption> menuItems = new List<MenuOption>();

            User user = HWManager.GetInstance().SettingManager.GetUser();

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

            if (user != null
                && user.UserType != null
                && (user.UserType.Equals("SuperUser", StringComparison.OrdinalIgnoreCase)
                    || user.UserType.Equals("Administrator", StringComparison.OrdinalIgnoreCase)))
            {
                menuItems.Add(new MenuOption()
                {
                    Title = "Review Tags",
                    TargetType = typeof(ReviewTagForm)
                });
            }

            menuItems.Add(new MenuOption()
            {
                Title = "About",
                TargetType = typeof(About)
            });

            menuItems.Add(new MenuOption()
            {
                Title = "Settings",
                TargetType = typeof(SettingsForm)
            });


            return menuItems;
        }
    }
}

