using System.Collections.Generic;

namespace HydrantWiki.Network
{
    public class HWRestRequest
    {
        public HWRestRequest()
        {
            Headers = new Dictionary<string, string>();
            ContentType = "application/json";
            Timeout = 30000;
        }

        public string Host { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public HWRestMethods Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public HWFile File { get; set; }

        public int Timeout { get; set; }
    }
}

