using System;
namespace HydrantWiki.Objects
{
    public class ChangePassword
    {
        public string Username { get; set; }

        public string ExistingPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
