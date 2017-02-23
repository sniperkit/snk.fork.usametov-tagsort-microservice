using Bookmarks.Common;
using Nancy;
using Nancy.SimpleAuthentication;
using Nancy.TinyIoc;
using SimpleAuthentication.Core;
using SimpleAuthentication.Core.Providers;
using System.Reflection;
using Nancy.Bootstrapper;
using System.Threading.Tasks;
using System;
using System.IO;

namespace TagSortService
{
    public class BookmarksBootstrapper : DefaultNancyBootstrapper
    {
        private const string TwitterConsumerKey = "qUlyMK0gfsIAMEbCxEYK66Ba9";
        private const string TwitterConsumerSecret = "T0Qmqwp0XFSU8zBFQWafvx7IfO2rT6scF0mNdfN7v1BKVDy81C";
        private const string FacebookAppId = "*key*";
        private const string FacebookAppSecret = "*secret*";
        private const string GoogleConsumerKey = "*key*";
        private const string GoogleConsumerSecret = "*secret*";

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

            var twitterProvider = new TwitterProvider(new ProviderParams { PublicApiKey = TwitterConsumerKey, SecretApiKey = TwitterConsumerSecret });
            var facebookProvider = new FacebookProvider(new ProviderParams { PublicApiKey = FacebookAppId, SecretApiKey = FacebookAppSecret });
            var googleProvider = new GoogleProvider(new ProviderParams { PublicApiKey = GoogleConsumerKey, SecretApiKey = GoogleConsumerSecret });

            var authenticationProviderFactory = new AuthenticationProviderFactory();

            try
            {
                authenticationProviderFactory.AddProvider(twitterProvider);
                authenticationProviderFactory.AddProvider(facebookProvider);
                authenticationProviderFactory.AddProvider(googleProvider);
            }
            catch (ReflectionTypeLoadException ex)
            {
                var loaderEx = ex.LoaderExceptions;
            }
            
            container.Register<IAuthenticationCallbackProvider>(new SocialAuthenticationCallbackProvider(context));
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            //Utils.SetupEnvironment();
            base.ApplicationStartup(container, pipelines);
            SetCurrentUserWhenLoggedIn(pipelines);            
        }

        private static void SetCurrentUserWhenLoggedIn(IPipelines pipelines)
        {
            pipelines.BeforeRequest += async (nancyContext, ct) =>
            {
                await Task.Run(() => 
                {                    
                    if (nancyContext.Request.Cookies.ContainsKey(Utils.SESSION_KEY))
                        nancyContext.CurrentUser = new TokenService()
                                                      .GetUserFromToken
                                                        (nancyContext.Request.Cookies[Utils.SESSION_KEY]);
                    else
                        nancyContext.CurrentUser = null;

                });

                return null;
            };
        }
    }
}