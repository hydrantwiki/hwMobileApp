using System;
namespace HydrantWiki.Objects
{
    public class ResetRequest
    {
        public string Email { get; set; }

        public Guid InstallId { get; set; }
    }
}
