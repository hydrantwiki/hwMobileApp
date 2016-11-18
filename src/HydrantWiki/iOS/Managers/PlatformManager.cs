using System;
using HydrantWiki.Interfaces;
using HydrantWiki.iOS.Helpers;
using HydrantWiki.Network;
using HydrantWiki.Objects;
using RestSharp;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Drawing;
using System.Linq;

namespace HydrantWiki.iOS.Managers
{
    public class PlatformManager : IPlatformManager
    {
        public PlatformManager()
        {
            string dataFolder = DataFolder;

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            string imagesFolder = ImageFolder;
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
        }

        public string DataFolder
        {
            get
            {
                string rootAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return Path.Combine(rootAppFolder, "Library", "HWMobile");
            }
        }

        public string ImageFolder
        {
            get
            {
                string rootAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return Path.Combine(rootAppFolder, "Library", "HWMobileImage");
            }
        }

        public bool HasNetworkConnectivity
        {
            get
            {
                NetworkStatus internetStatus = Reachability.InternetConnectionStatus();

                if (internetStatus == NetworkStatus.NotReachable)
                {
                    return false;
                }

                return true;
            }
        }

        public string ApiHost
        {
            get
            {
                //return "https://mobileapi.hydrantwiki.com";
                return "http://192.168.50.192:8990";
            }
        }

        public string GetLocalImageFilename(string _filename)
        {
            string rootAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string jpgFilename = Path.Combine(ImageFolder, _filename);
            return jpgFilename;
        }

        public async Task SaveImage(ImageSource _imageSource, string _filename)
        {
            var renderer = new StreamImagesourceHandler();

            var photo = await renderer.LoadImageAsync(_imageSource);

            string jpgFilename = GetLocalImageFilename(_filename);

            NSData imgData = photo.AsJPEG();
            NSError err = null;

            if (imgData.Save(jpgFilename, false, out err))
            {
                Console.WriteLine("saved as " + jpgFilename);
            } else {
                Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
            }
        }

        public HWRestResponse SendRestRequest(HWRestRequest _request)
        {
            //TODO save client in dictionary based on host
            RestClient client = new RestClient(_request.Host);

            RestRequest request = new RestRequest(_request.Path, GetMethod(_request.Method));
            request.Timeout = _request.Timeout;

            //Process Headers
            foreach (var key in _request.Headers.Keys)
            {
                request.AddHeader(key, _request.Headers[key]);
            }

            //Add Body
            if (_request.Body != null)
            {
                request.AddParameter("application/json", _request.Body, ParameterType.RequestBody);
            }

            if (_request.File != null
                && File.Exists(_request.File.FullPathFilename))
            {
                request.AddFile("fileData", _request.File.FullPathFilename, _request.File.Filename);
                request.AlwaysMultipartFormData = true;
            }

            //Execute Rest Method
            var response = client.Execute(request);

            //Create Response
            HWRestResponse atResponse = new HWRestResponse();

            //Process Headers, set status and body
            foreach (var parameter in response.Headers)
            {
                atResponse.Headers.Add(parameter.Name, parameter.Value.ToString());
            }
            atResponse.Status = GetStatus(response.ResponseStatus);
            atResponse.Body = response.Content;
            atResponse.ErrorMessage = response.ErrorMessage;

            return atResponse;
        }

        private HWResponseStatus GetStatus(ResponseStatus status)
        {
            switch (status)
            {
            case ResponseStatus.None:
                return HWResponseStatus.None;
            case ResponseStatus.Aborted:
                return HWResponseStatus.Aborted;
            case ResponseStatus.Completed:
                return HWResponseStatus.Completed;
            case ResponseStatus.Error:
                return HWResponseStatus.Error;
            case ResponseStatus.TimedOut:
                return HWResponseStatus.Error;
            }

            throw new ArgumentException("Unexpected rest status");
        }

        private Method GetMethod(HWRestMethods method)
        {
            switch (method)
            {
            case HWRestMethods.Post:
                return Method.POST;
            case HWRestMethods.Get:
                return Method.GET;
            case HWRestMethods.Delete:
                return Method.DELETE;
            }

            throw new ArgumentException("Unexpected rest method");
        }

        public string GetLocalThumbnailFilename(string _filename)
        {
            string rootAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string jpgFilename = Path.Combine(rootAppFolder, "Library", "HWMobileThumbnail", _filename);
            return jpgFilename;
        }

        public void GenerateThumbnail(string _imageFilename, string _thumbnailFilename)
        {
            byte[] imageData = File.ReadAllBytes(_imageFilename);

            byte[] thumbnailData = ResizeImage(imageData, 60, 80);

            File.WriteAllBytes(_thumbnailFilename, thumbnailData);
        }

        public static byte[] ResizeImage(byte[] imageData, float maxDimension, int quality)
        {
            UIImage originalImage = ImageFromByteArray(imageData);


            float oldWidth = (float)originalImage.Size.Width;
            float oldHeight = (float)originalImage.Size.Height;
            float scaleFactor = 0f;

            if (oldWidth > oldHeight)
            {
                scaleFactor = maxDimension / oldWidth;
            } else
            {
                scaleFactor = maxDimension / oldHeight;
            }

            float newHeight = oldHeight * scaleFactor;
            float newWidth = oldWidth * scaleFactor;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                (int)newWidth, (int)newHeight, 8,
                (int)(4 * newWidth), CGColorSpace.CreateDeviceRGB(),
                CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, newWidth, newHeight);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIImage resizedImage = UIImage.FromImage(context.ToImage());

                // save the image as a jpeg
                return resizedImage.AsJPEG((float)quality).ToArray();
            }
        }

        public static UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}
