using System.Configuration;
using Bookmarks.Common;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TagSortService
{
    public static class Utils
    {
        public const string UNDEFINED = "undefined";

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

            return GetConfig().First(s => s.IsSomething()).Value;
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

        public static IEnumerable<Maybe<string>> GetConfig()
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