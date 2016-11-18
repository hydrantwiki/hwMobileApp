using System;
using Newtonsoft.Json.Linq;

namespace HydrantWiki.Objects
{
    public class Auth0UserX
    {
        public string Auth0AccessToken { get; set; }
        public string IdToken { get; set; }
        public JObject Profile { get; set; }
        public string RefreshToken { get; set; }
    }
}
