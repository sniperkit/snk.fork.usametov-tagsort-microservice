using Bookmarks.Common;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace TagSortService.Tests
{
    public class BookmarkCollectionRepositoryTests
    {
        [Fact]
        public void Should_answer_200_onRootPath()
        {
            var sut = new Browser(new BookmarksBootstrapper());

            var actual = sut.Get("/");

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public void Shoud_get_recycling_bookmarks()
        {
            var sut = new Browser(new BookmarksBootstrapper());

            var actual = sut.Get("/bookmarksByTagBundle/recycling/0/3000/");

            Assert.True(actual.Body.DeserializeJson<Bookmark[]>().Length > 0);
        }
    }
}
