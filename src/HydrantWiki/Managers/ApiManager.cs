using System;
using HydrantWiki.Network;
using HydrantWiki.Objects;
using HydrantWiki.ResponseObjects;
using Newtonsoft.Json;

namespace HydrantWiki.Managers
{
    public class ApiManager
    {
        private HWManager m_HWManager;

        public ApiManager(HWManager _manager)
        {
            m_HWManager = _manager;
        }

        public User Authenticate(string _username, string _password)
        {
            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/authorize";
            request.Headers.Add("Username", _username);
            request.Headers.Add("Password", _password);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            AuthenticationResponse responseObject =
                JsonConvert.DeserializeObject<AuthenticationResponse>(response.Body);

            if (responseObject.Success)
            {
                return responseObject.User;
            }

            return null;
        }
    }
}
