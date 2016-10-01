using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using HydrantWiki.iOS.Managers;
using UIKit;
using XLabs.Forms;
using XLabs.Forms.Services;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Geolocation;

namespace HydrantWiki.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            SetIoc();

            string rootAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dataFolder = Path.Combine(rootAppFolder, "Library", "HWMobile");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            LoadApplication(new HydrantWikiApp(dataFolder, new PlatformManager()));

            return base.FinishedLaunching(app, options);
        }

        private void SetIoc()
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            var resolverContainer = new global::XLabs.Ioc.SimpleContainer();

            var app = new XFormsAppiOS();
            app.Init(this);

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice);
            resolverContainer.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            resolverContainer.Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()));
            resolverContainer.Register<IGeolocator>(t => new Geolocator());

            XLabs.Ioc.Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
