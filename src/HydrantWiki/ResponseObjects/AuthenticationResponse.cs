using System;
using HydrantWiki.Objects;

namespace HydrantWiki.ResponseObjects
{
    public class AuthenticationResponse
    {
        public User User { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
