/*
Sniperkit-Bot
- Status: analyzed
*/

﻿using Nancy.Security;
using System;

namespace TagSortService
{
    public class TokenService
    {        
        public string GetToken(string userName)
        {
            return Utils.EncryptUsername(userName);
        }

        public IUserIdentity GetUserFromToken(string token)
        {
            try
            {
                var userName = Utils.DecryptUsername(token).TrimEnd();
                return new User { UserName = userName };
            }
            catch (Exception ex)
            {
                var logInfo = ex.GetType().Name;                
            }

            return null;
        }
    }
}