using System;
using System.Collections.Generic;

namespace TagSortService
{
    public class TokenPayload
    {
        public string UserName { get; set; }

        public DateTime Expires { get; set; }

        public IEnumerable<string> Claims
        {
            get; set;
        }
    }
}