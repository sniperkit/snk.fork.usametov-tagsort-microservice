using System;
using Nancy.Security;
using Jose;

namespace TagSortService
{
    public class JwtTokenizer : ITokenizer
    {
        public string Tokenize(IUserIdentity user)
        {
            var secretKey = Utils.GetAppSetting("FBClientSecret");//TODO: cache it 
            return JWT.Encode(new TokenPayload
                {
                    UserName = user.UserName,
                    Expires = DateTime.Now.AddMinutes(40),
                    Claims = user.Claims
                }
                , secretKey, JweAlgorithm.A256GCMKW, 
                JweEncryption.A256CBC_HS512);
        }
    }
}