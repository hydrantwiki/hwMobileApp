using System;
using System.Collections.Generic;

namespace HydrantWiki.Network
{
    public class HWRestResponse
    {
        public HWRestResponse()
        {
            Headers = new Dictionary<string, string>();
        }

        public HWResponseStatus Status { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public string ErrorMessage { get; set; }

    }
}

