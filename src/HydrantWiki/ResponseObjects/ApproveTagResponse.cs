using System;
namespace HydrantWiki.ResponseObjects
{
    public class ApproveTagResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }
    }
}
