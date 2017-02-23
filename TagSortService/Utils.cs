using System.Configuration;
using Bookmarks.Common;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Yort.Otp;
using System.Dynamic;

namespace TagSortService
{
    public static class Utils
    {
        public const string UNDEFINED = "undefined";
        private const int _keySize = 256;
        private const string SEPARATOR = "^-^";
        public const string SESSION_KEY = "TagSortServiceUser";

        /// <summary>
        /// this will throw InvalidOperationException in case if connection string has not been set up
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            #region old code
            //ConnectionStringsSection section =
            //    ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;

            //return
            //       !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MONGOLAB_URI"]) ?
            //       ConfigurationManager.AppSettings["MONGOLAB_URI"] :
            //       !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MONGOHQ_URL"]) ?
            //       ConfigurationManager.AppSettings.Get("MONGOHQ_URL") :
            //       section.ConnectionStrings.Count > 0 ?
            //       section.ConnectionStrings[0].ConnectionString : null;
            #endregion

            return GetDBConnectionConfig().First(s => s.IsSomething()).Value;
        }

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string DeriveOneTimeKey(string nonce)
        {            
            var hashBytes = new Sha512HashAlgorithm().ComputeHash
                ( Encoding.UTF8.GetBytes(GetConnectionString())
                , Encoding.UTF8.GetBytes(nonce));

            return Encoding.UTF8.GetString(hashBytes);
        }

        public static string EncryptUsername(string userName)
        {
            var nonce = DateTime.Now.ToLongTimeString();
            var oneTimeTicket = DeriveOneTimeKey(nonce);

            var key = Encoding.UTF8.GetBytes(oneTimeTicket)
                                    .Take(_keySize / 8).ToArray();

            var secret = Encoding.UTF8.GetBytes(userName.PadRight(_keySize / 8));                        

            return Convert.ToBase64String(key.Zip(secret, (b1, b2) => (byte)(b1 ^ b2)).ToArray()) 
                + SEPARATOR + nonce;
        }
        
        public static string DecryptUsername(string encryptedUserName)
        {
            var context = encryptedUserName.Split
                (new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            var key = Encoding.UTF8.GetBytes(DeriveOneTimeKey(context[1]))
                                    .Take(_keySize / 8).ToArray();

            var secret = Convert.FromBase64String(context[0].PadRight(_keySize / 8));

            return Encoding.UTF8.GetString(key.Zip(secret, (b1, b2) => (byte)(b1 ^ b2)).ToArray()); 
        }
              
        public static string[] ToStringArray(this TagCount[] tagCounts) 
        {
            return tagCounts.Select(t => t.Tag).ToArray();
        }

        public static Maybe<string> GetMaybeFromString(this string s)
        {
            return (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                ? Maybe<string>.Nothing
                : s.ToMaybe();
        }

        public static IEnumerable<Maybe<string>> GetDBConnectionConfig()
        {
            ConnectionStringsSection section =
                ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;
                        
            return new List<Maybe<string>> {
                ConfigurationManager.AppSettings["MONGOLAB_URI"].GetMaybeFromString()
            ,
                ConfigurationManager.AppSettings.Get("MONGOHQ_URL").GetMaybeFromString()
            ,
                section.ConnectionStrings.Count > 0 
                ? section.ConnectionStrings[0].ConnectionString.GetMaybeFromString() 
                : Maybe<string>.Nothing
              };            
        }
    }
}