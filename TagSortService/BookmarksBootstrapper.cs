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
using Nancy.Diagnostics;
using Nancy.Conventions;

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

            var twitterProvider = new TwitterProvider(new ProviderParams { PublicApiKey = Utils.GetAppSetting("TwitterConsumerKey"), SecretApiKey = Utils.GetAppSetting("TwitterConsumerSecret") });
            var facebookProvider = new FacebookProvider(new ProviderParams { PublicApiKey = Utils.GetAppSetting("FBClientId"), SecretApiKey = Utils.GetAppSetting("FBClientSecret") });
            var googleProvider = new GoogleProvider(new ProviderParams { PublicApiKey = Utils.GetAppSetting("GoogleConsumerKey"), SecretApiKey = Utils.GetAppSetting("GoogleConsumerSecret") });

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
            //DiagnosticsHook.Disable(pipelines);
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
        /// <summary>
        /// this is for debugging only
        /// </summary>
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"ik.puk.tra.la.la" }; }
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            pipelines.AfterRequest.AddItemToEndOfPipeline(async (ctx, ct) => {
                await Task.Run(()=>
                {
                    ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
                });
            });
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add
                (StaticContentConventionBuilder.AddDirectory("bookmarks", "MiGG"));

        }
        
    }
}