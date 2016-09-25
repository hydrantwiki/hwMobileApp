using System;
using System.Collections.Generic;
using System.IO;

namespace HydrantWiki.Network
{
    public class HWRestRequest
    {
        public HWRestRequest()
        {
            Headers = new Dictionary<string, string>();
        }

        public string Host { get; set; }
        public string Path { get; set; }
        public HWRestMethods Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public HWFile File { get; set; }
    }
}

