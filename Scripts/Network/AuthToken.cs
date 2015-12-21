using System;

namespace DRG.Network
{
    public class AuthToken
    {

        private DateTime ExpireDate;

        public string Token { get; private set; }

        public bool IsActive
        {
            get
            {
                return ExpireDate < DateTime.UtcNow;
            }
        }

        public AuthToken(string token, DateTime expireDate)
        {
            Token = token;
            ExpireDate = expireDate;
        }

        private AuthToken()
        {

        }
    }
}
