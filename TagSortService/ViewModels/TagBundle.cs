/*
Sniperkit-Bot
- Status: analyzed
*/

﻿using Bookmarks.Common;

namespace TagSortService.ViewModels
{    
    public class TagBundle 
    {        
        public TagCount[] ExcludeTags
        {
            get;
            set;
        }
                
        public string[] ExcludeTagBundles
        {
            get;
            set;
        }
                
        public string Id
        {
            get;
            set;
        }
        
        public string Name
        {
            get;
            set;
        }

        public TagCount[] Tags
        {
            get;
            set;
        }        
    }
}