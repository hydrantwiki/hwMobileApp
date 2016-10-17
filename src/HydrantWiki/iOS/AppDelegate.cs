using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using HydrantWiki.iOS.Managers;
using LiteDB;
using LiteDB.Platform;
using UIKit;
using XLabs.Forms;
using XLabs.Forms.Services;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Email;
using XLabs.Platform.Services.Geolocation;
using XLabs.Platform.Services.Media;

namespace HydrantWiki.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            SetIoc();

            LoadApplication(new HydrantWikiApp(new PlatformManager()));

            return base.FinishedLaunching(app, options);
        }

        private void SetIoc()
        {
            Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();

            LitePlatform.Initialize(new LitePlatformiOS());

            var app = new XFormsAppiOS();
            app.Init(this);

            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()))
                .Register<IGeolocator>(t => new Geolocator())
                .Register<ITextToSpeechService, TextToSpeechService>()
                .Register<IEmailService, EmailService>()
                .Register<IMediaPicker, MediaPicker>()
                .Register<ISecureStorage, SecureStorage>()
                .Register<IDependencyContainer>(t => resolverContainer);

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
