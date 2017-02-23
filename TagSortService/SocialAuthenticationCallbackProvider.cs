using Bookmarks.Common;
using Nancy;
using Nancy.Cookies;
using Nancy.SimpleAuthentication;
using System;
using System.Security.Cryptography;

namespace TagSortService
{
    public class SocialAuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        IBookmarksContext context;
        IBookmarksContext BookmarksContext
        {
            get
            {
                return context;
            }
        }

        

        public SocialAuthenticationCallbackProvider(IBookmarksContext bookmarkContext)
        {
            context = bookmarkContext;
        }

        public dynamic Process(NancyModule module, AuthenticateCallbackData callback)
        {
            module.Context.CurrentUser = new User { UserName = callback.AuthenticatedClient.UserInformation.UserName };

            var loggedInUser = BookmarksContext.GetUserByUsername(module.Context.CurrentUser.UserName);

            //TODO: add logging here
            if (loggedInUser == null)
            {
                module.Context.CurrentUser = null;
                return OnRedirectToAuthenticationProviderError(module, "account doesn't exist in our system!");
            }

            return module.Response
              .AsRedirect("/")
              .WithCookie(new NancyCookie(Utils.SESSION_KEY, new TokenService().GetToken(module.Context.CurrentUser.UserName)));
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            return "login failed: " + errorMessage;
        }
    }
}