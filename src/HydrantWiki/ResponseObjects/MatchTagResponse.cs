using System;
namespace HydrantWiki.ResponseObjects
{
    public class MatchTagResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }
    }
}
