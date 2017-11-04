using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagSortService
{
    public class AuthModule : NancyModule
    {
        public AuthModule(ITokenizer tokenizer, IUserRepository userRepository)
            : base("/login")
        {
            Post["/"] = x =>
            {
                var userName = (string)this.Request.Form["username"];
                var password = (string)this.Request.Form["password"];
                                
                if (!userRepository.ValidateUser(userName, password))
                {
                    return HttpStatusCode.Unauthorized;
                }

                var userIdentity = userRepository.GetUserByUserName(userName);

                var token = tokenizer.Tokenize(userIdentity);

                return new
                {
                    Token = token,
                };
            };
        }
    }
}