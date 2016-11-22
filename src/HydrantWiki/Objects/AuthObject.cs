using System;
namespace HydrantWiki.Objects
{
    public class AuthObject
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Guid InstallId { get; set; }
    }
}
