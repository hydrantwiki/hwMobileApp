using System.Collections.Generic;
using HydrantWiki.Objects;

namespace HydrantWiki.ResponseObjects
{
    public class HydrantQueryResponse
    {
        public bool Success { get; set; }

        public List<Hydrant> Hydrants { get; set; }
    }
}
