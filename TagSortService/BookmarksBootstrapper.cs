using Bookmarks.Common;
using Nancy;
using Nancy.TinyIoc;
using Nancy.Bootstrapper;
using System.Threading.Tasks;
using System;
using Nancy.Diagnostics;
using Nancy.Conventions;
using Nancy.Authentication.Stateless;
using Jose;

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
            container.Register<ITokenizer>(new JwtTokenizer());
            //TODO: add IUserRepository
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            //DiagnosticsHook.Disable(pipelines);
            base.ApplicationStartup(container, pipelines);
            //SetCurrentUserWhenLoggedIn(pipelines);            
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
            var secretKey = Utils.GetAppSetting("FBClientSecret");//TODO: cache it 
                      
            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                {
                    var jwtToken = nancyContext.Request.Headers.Authorization;

                    try
                    {
                        var payload = JWT.Decode<TokenPayload>(jwtToken, secretKey, 
                            JweAlgorithm.A256GCMKW, JweEncryption.A256CBC_HS512);

                        if (payload.Expires > DateTime.UtcNow)
                        {                            
                            return new User { UserName = payload.UserName, Claims = payload.Claims };
                        }

                        return null;


                    }
                    catch (Exception)
                    {
                        return null;
                    }                    
                });
            
            pipelines.AfterRequest.AddItemToEndOfPipeline(async (ctx, ct) => {
                await Task.Run(()=>
                {
                    ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
                });
            });

            StatelessAuthentication.Enable(pipelines, configuration);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            
            nancyConventions.StaticContentsConventions.Add
                (StaticContentConventionBuilder.AddFile("/bookmarks/security", "../../javascript/MiGG-ng2/"));

            nancyConventions.StaticContentsConventions.Add
                (StaticContentConventionBuilder.AddFile("/bookmarks/cryptography", "../../javascript/MiGG-ng2/"));

            nancyConventions.StaticContentsConventions.Add
                (StaticContentConventionBuilder.AddFile("/bookmarks/books", "../../javascript/MiGG-ng2/"));

            nancyConventions.StaticContentsConventions.Add
                (StaticContentConventionBuilder.AddFile("/bookmarks", "../../javascript/MiGG-ng2/"));
            
        }

    }
}