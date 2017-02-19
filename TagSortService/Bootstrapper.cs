using Bookmarks.Common;
using Nancy;
using Nancy.TinyIoc;

namespace TagSortService
{
    public class BookmarksBootstrapper : DefaultNancyBootstrapper
    {
        public string ConnectionString
        {
            get
            {
                return Utils.GetConnectionString();
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var context = new Bookmarks.Mongo.Data.BookmarksContext(ConnectionString);
            container.Register<IBookmarksContext>(context);
        }
    }
}