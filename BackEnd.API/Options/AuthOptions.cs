using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BackEnd.API.Options
{
    public class AuthOptions
    {
        public const string ISSUER = "TangramServer";
        public const string AUDIENCE = "TangramClient";
        public const string KEY = "d3b2cc0c-db8c-4dec-a25a-e1179ac30025";
        public const int LIFETIME = 2;
        public const int REFRESH_VALID_DAYS = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
