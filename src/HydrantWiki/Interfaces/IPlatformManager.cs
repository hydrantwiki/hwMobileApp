using System;
using System.Threading.Tasks;
using HydrantWiki.Network;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Interfaces
{
    public interface IPlatformManager
    {
        /// <summary>
        /// Sends a rest request.
        /// </summary>
        /// <returns>The rest request.</returns>
        /// <param name="_request">Request.</param>
        HWRestResponse SendRestRequest(HWRestRequest _request);

        /// <summary>
        /// Checks to see cref="if the device has network connectivity
        /// </summary>
        /// <value><c>true</c> if has network connectivity; otherwise, <c>false</c>.</value>
        bool HasNetworkConnectivity { get; }

        string ApiHost { get; }

        string GetLocalImageFilename(string _filename);

        string GetLocalThumbnailFilename(string _filename);

        Task SaveImage(ImageSource _imageSource, string _filename);

        void GenerateThumbnail(string _imageFilename, string _thumbnailFilename);

        string DataFolder { get; }

        string ImageFolder { get; }

    }
}
