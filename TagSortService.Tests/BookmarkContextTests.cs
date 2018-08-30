/*
Sniperkit-Bot
- Status: analyzed
*/

﻿using Bookmarks.Common;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using System.Collections.Generic;
using Xunit;

namespace TagSortService.Tests
{
    public class BookmarkContextTests
    {
        private readonly IBookmarksContext fakeBookmarksContext;
        private Browser sut;
        //private readonly Bookmark bookmarks;
        private readonly TagBundle tagBundle;

        public BookmarkContextTests()
        {
            fakeBookmarksContext = A.Fake<IBookmarksContext>();
            sut = new Browser(with =>
            {
                with.Dependency(fakeBookmarksContext);
                with.Module<BookmarkCollectionRepository>();
            });

            tagBundle = new TagBundle { Name = "linux" };
            //bookmarks = new Bookmark()
            //{
            //    Id = "57146c5f083989dcf1e69c4c"
            //    , LinkText = "LinuxCNC"
            //    , LinkUrl = "http://www.linuxcnc.org/"
            //    , Tags = new List<string>() { "cnc", "3dprinting", "linux", "opensource" }
            //};
        }

        [Fact]
        public void Should_call_GetBookmarksByTagBundle()
        {
            sut.Get(string.Format("/bookmarksByTagBundle/{0}/0/3/", tagBundle.Name));

            AssertCalledBookmarksContext(tagBundle);
        }

        private void AssertCalledBookmarksContext(TagBundle expected)
        {
            A.CallTo(() =>
              fakeBookmarksContext.GetBookmarksByTagBundle(expected.Name
                , 0, 3))
              .MustHaveHappened();
        }
    }
}
