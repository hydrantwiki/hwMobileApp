using System;
using HydrantWiki.Network;
using HydrantWiki.Objects;

namespace HydrantWiki.Interfaces
{
    public interface IPlatformManager
    {
        HWRestResponse SendRestRequest(HWRestRequest _request);

        bool HasNetworkConnectivity { get; }

        GeoPoint GetLocation();
    }
}
