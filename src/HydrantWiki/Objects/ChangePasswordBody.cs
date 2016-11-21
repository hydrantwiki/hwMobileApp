namespace HydrantWiki.Objects
{
    public class ChangePasswordBody
    {
        public string Username { get; set; }

        public string ExistingPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
