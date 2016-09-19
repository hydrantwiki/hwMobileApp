using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using HydrantWiki.iOS.Managers;
using UIKit;

namespace HydrantWiki.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            string rootAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dataFolder = Path.Combine(rootAppFolder, "Library", "ATMobile");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            LoadApplication(new HydrantWikiApp(dataFolder, new PlatformManager()));

            return base.FinishedLaunching(app, options);
        }
    }
}
