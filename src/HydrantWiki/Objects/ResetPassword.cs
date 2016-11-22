using System;
namespace HydrantWiki.Objects
{
    public class ResetPassword
    {
        public string Email { get; set; }

        public string Code { get; set; }

        public string NewPassword { get; set; }

        public Guid InstallId { get; set; }
    }
}
