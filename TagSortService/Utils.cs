using System.Configuration;
using Bookmarks.Common;
using System.Linq;

namespace TagSortService
{
    public static class Utils
    {
        public const string UNDEFINED = "undefined";

        public static string GetConnectionString()
        {            
            ConnectionStringsSection section =
                ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;

            return
                   !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MONGOLAB_URI"]) ?
                   ConfigurationManager.AppSettings["MONGOLAB_URI"] :
                   !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MONGOHQ_URL"]) ?
                   ConfigurationManager.AppSettings.Get("MONGOHQ_URL") :
                   section.ConnectionStrings.Count > 0 ?
                   section.ConnectionStrings[0].ConnectionString : null;
        }

        public static string[] ToStringArray(this TagCount[] tagCounts) 
        {
            return tagCounts.Select(t => t.Tag).ToArray();
        }
    }
}