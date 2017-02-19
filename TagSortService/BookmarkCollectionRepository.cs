using Bookmarks.Common;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagSortService
{
    public class BookmarkCollectionRepository : NancyModule
    {        
        IBookmarksContext context;
        IBookmarksContext BookmarksContext
        {
            get
            {
                return context;
            }
        }

        public BookmarkCollectionRepository(IBookmarksContext bookmarkContext)
        {
            context = bookmarkContext;

            Get["/"] = _ => "Welcome to TagSortService Home page ";

            Get["/termcounts/{bufferSize:int}"] = 
                parameters => Response.AsJson(CalculateTermCounts((int)parameters.bufferSize));

            Get["/bookmarkCollections/"] = _ => Response.AsJson(GetBookmarkCollections());

            Get["/tagBundleNames/{bookmarksCollectionId}"] = 
                parameters => Response.AsJson(GetTagBundleNames((string)parameters.bookmarksCollectionId));

            Get["/tagBundle/{id}"] =
                parameters => Response.AsJson(GetTagBundleById((string)parameters.id));

            Get["/NextMostFrequentTags/{id}/{limitTermCounts:int}/{exclTagBundles}"] =
                parameters => Response.AsJson(GetNextMostFrequentTags((string)parameters.id                                                                    
                                                                    , (int)parameters.limitTermCounts
                                                                    , (string)parameters.exclTagBundles));

            Get["/AssociatedTerms/{tagBundleId}/{bufferSize}"] =
                parameters => Response.AsJson(GetAssociatedTerms((string)parameters.tagBundleId
                                                               , (int)parameters.bufferSize));

            Post["/tagBundle/updateById"] = ctx =>
                                               {
                                                   var tagBundle = this.Bind<ViewModels.TagBundle>();

                                                   UpdateTagBundleById(new TagBundle
                                                   {
                                                       Id = tagBundle.Id
                                                       ,
                                                       Name = tagBundle.Name
                                                       ,
                                                       Tags = MapTags(tagBundle.Tags)
                                                       ,
                                                       ExcludeTags = MapTags(tagBundle.ExcludeTags)
                                                       ,
                                                       ExcludeTagBundles = tagBundle.ExcludeTagBundles
                                                   });
                                                                                                      
                                                   return HttpStatusCode.OK;                                                   
                                               };

            Post["/tagBundle/create"] = ctx =>
                                            {
                                                var tagBundle = this.Bind<TagBundle>();
                                                CreateTagBundle(tagBundle);
                                                return HttpStatusCode.OK;
                                            };

            Post["/tagBundle/editName"] = ctx =>
                                            {
                                                var tagBundle = this.Bind<TagBundle>();
                                                UpdateTagBundleNameById(tagBundle);
                                                return HttpStatusCode.OK;
                                            };

            Get["/bookmarksByTagBundle/{tagBundleName}/{skip}/{take}/"] =
                parameters => Response.AsJson(GetBookmarksByTagBundle((string)parameters.tagBundleName
                                                                    , (int)parameters.skip
                                                                    , (int)parameters.take));
        }

        public IEnumerable<TagCount> CalculateTermCounts(int bufferSize)
        {
            return bufferSize > 0
                ? BookmarksContext.CalculateTermCounts(bufferSize)
                : BookmarksContext.CalculateTermCounts(Bookmarks.Mongo.Data.BookmarksContext.TAG_COUNTS_PAGE_SIZE);
        }
        
        public IEnumerable<BookmarksCollections> GetBookmarkCollections()
        {
            return BookmarksContext.GetBookmarksCollections().Select
                (bc => new BookmarksCollections
                {
                    Id = bc.Id
                    ,
                    Name = bc.Name
                });
        }

        public void CreateBookmarksCollection(string name)
        {
            BookmarksContext.CreateBookmarksCollection(name);
        }


        public void CreateTagBundle(TagBundle tagBundle)
        {
            BookmarksContext.CreateTagBundle(tagBundle);
        }

        public IEnumerable<Bookmark> GetBookmarksByTagBundle(string tagBundleName, int skip, int take)
        {
            return BookmarksContext.GetBookmarksByTagBundle(tagBundleName, skip, take);
        }
        
        //public IEnumerable<Bookmark> GetBookmarksByTagBundle(string tagBundleName, int? skip, int? take)
        //{
        //    return BookmarksContext.GetBookmarksByTagBundle(tagBundleName, skip, take);
        //}

        public IEnumerable<TagCount> GetNextMostFrequentTags
            (string tagBundleId, int limitTermCounts, string excludeTagBundleNames)
        {
            string[] excludeTagBundles = new string[0];
            if (!string.IsNullOrEmpty(excludeTagBundleNames))
                excludeTagBundles = excludeTagBundleNames.Split
                    (new char[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            return BookmarksContext.GetNextMostFrequentTags(tagBundleId, excludeTagBundles, limitTermCounts);
        }

        public ViewModels.TagBundle GetTagBundleById(string objId)
        {
            if (string.IsNullOrEmpty(objId) || objId.Equals(Utils.UNDEFINED))
                throw new ArgumentNullException("objId");

            var bundle = BookmarksContext.GetTagBundleById(objId);

            return new ViewModels.TagBundle
            {
                Name = bundle.Name
                ,
                Id = bundle.Id
                ,
                Tags = MapTags(bundle.Tags)
                ,
                ExcludeTags = MapTags(bundle.ExcludeTags)
                ,
                ExcludeTagBundles = bundle.ExcludeTagBundles
            };
        }

        private TagCount[] MapTags(string[] tags)
        {
            return tags.Distinct().Select(t => new TagCount { Tag = t, Count = -1 })
                       .OrderBy(t => t.Tag).ToArray();
        }

        private string[] MapTags(TagCount[] tags)
        {
            return tags != null ? tags.Distinct().Select(t => t.Tag).ToArray()
                                : new string[0];
        }

        public void UpdateTagBundleById(TagBundle tagBundle)
        {
            BookmarksContext.UpdateTagBundleById(tagBundle);
        }

        public void UpdateExcludeList(TagBundle tagBundle)
        {
            BookmarksContext.UpdateExcludeList(tagBundle.Id, tagBundle.ExcludeTags);
        }
                
        public IEnumerable<TagCount> GetAssociatedTerms(string objId, int bufferSize)
        {
            var tagBundle = BookmarksContext.GetTagBundleById(objId);
            return BookmarksContext.GetAssociatedTerms(tagBundle, bufferSize);
        }

        public IEnumerable<TagBundle> GetTagBundleNames(string bookmarksCollectionId)
        {
            if (bookmarksCollectionId.Equals(Utils.UNDEFINED))
                bookmarksCollectionId = string.Empty;

            return BookmarksContext.GetTagBundleNames(bookmarksCollectionId).Select(tb => new TagBundle
            {
                Id = tb.Id
                ,
                Name = tb.Name
            });
        }

        public void UpdateTagBundleNameById(TagBundle tagBundle)
        {
            BookmarksContext.EditTagBundle(tagBundle.Id, tagBundle.Name);
        }


        public IEnumerable<TagCount> GetRemainingTermCounts(int bufferSize, string excludeTagBundleNames)
        {
            string[] excludeTagBundles = new string[0];
            if (!string.IsNullOrEmpty(excludeTagBundleNames))
                excludeTagBundles = excludeTagBundleNames.Split
                    (new char[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            return bufferSize > 0
                ? BookmarksContext.CalculateRemainingTermCounts(bufferSize, excludeTagBundles)
                : BookmarksContext.CalculateRemainingTermCounts
                                        (Bookmarks.Mongo.Data.BookmarksContext.TAG_COUNTS_PAGE_SIZE
                                       , excludeTagBundles);
        }

        public IEnumerable<Bookmark> ExportBookmarks()
        {
            return BookmarksContext.BackupBookmarks();
        }


        public void UpdateExcludeTagBundlesList(TagBundle tagBundle)
        {
            BookmarksContext.UpdateExcludeTagBundlesList(tagBundle.Id, tagBundle.ExcludeTagBundles);
        }
    }
}