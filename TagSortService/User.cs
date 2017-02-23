using System.Collections.Generic;
using Nancy.Security;

namespace TagSortService
{
    public class User : IUserIdentity
    {
        public string Id { get; set; }
                
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string UserName
        {
            get; set;
        }

        public IEnumerable<string> Claims
        {
            get; private set;
        }
    }
}