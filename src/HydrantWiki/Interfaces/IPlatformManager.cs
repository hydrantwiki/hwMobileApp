using System;
using System.Threading.Tasks;
using HydrantWiki.Network;
using HydrantWiki.Objects;

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
    }
}
