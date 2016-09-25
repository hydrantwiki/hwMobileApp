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

namespace HydrantWiki.iOS.Managers
{
    public class PlatformManager : IPlatformManager
    {

        public PlatformManager()
        {
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
                return "http://mobileapi.hydrantwiki.com";
            }
        }

        public string GetLocalImageFilename(string _filename)
        {
            string rootAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string jpgFilename = Path.Combine(rootAppFolder, "Library", "HWMobileImage", _filename);
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

            //Process Headers
            foreach (var key in _request.Headers.Keys)
            {
                request.AddHeader(key, _request.Headers[key]);
            }

            //Add Body
            if (_request.Body != null)
            {
                request.AddBody(_request.Body);
            }

            if (_request.File != null)
            {
                request.AddFile("fileData", _request.File.FileStream.CopyTo, _request.File.Filename);
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


    }
}
