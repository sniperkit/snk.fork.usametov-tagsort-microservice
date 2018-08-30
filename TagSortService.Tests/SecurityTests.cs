/*
Sniperkit-Bot
- Status: analyzed
*/

﻿using Nancy;
using Nancy.SimpleAuthentication;
using Nancy.Testing;
using SimpleAuthentication.Core;
using Xunit;
using TagSortService;
using System.Linq;
using Bookmarks.Common;
using FakeItEasy;
using Nancy.Security;

namespace TagSortService.Tests
{
    public class SecurityTests
    {
        private readonly IBookmarksContext fakeBookmarksContext;

        public SecurityTests()
        {
            fakeBookmarksContext = A.Fake<IBookmarksContext>();
            
        }

        [Fact]
        public void Should_login_successfully()
        {
            new Browser(with =>
            {
                with.Dependency(fakeBookmarksContext);
                with.Module<TestingModule>();
            }).Get("/testing");

            var callbackData = new AuthenticateCallbackData
            {
                AuthenticatedClient = new AuthenticatedClient("")
                {
                    UserInformation = new UserInformation { UserName = "test_user" }
                }
            };

            var userNameToken = new TokenService().GetToken("test_user");
            //make fake context to return something
            A.CallTo(() => fakeBookmarksContext.GetUserByUsername("test_user"))
             .Returns(new Bookmarks.Common.User { Name = "test_user" });
            //using fake context
            var sut = new SocialAuthenticationCallbackProvider(fakeBookmarksContext);

            var actual = (Response)sut.Process(TestingModule.actualModule, callbackData);

            Assert.Equal(HttpStatusCode.SeeOther, actual.StatusCode);
            Assert.Contains("TagSortServiceUser", actual.Cookies.Select(cookie => cookie.Name));
            Assert.Contains(userNameToken, actual.Cookies.Select(cookie => cookie.Value));
        }

        [Fact]
        public void Should_login_fail()
        {
            new Browser(with =>
            {
                with.Dependency(fakeBookmarksContext);
                with.Module<TestingModule>();
            }).Get("/testing");

            var callbackData = new AuthenticateCallbackData
            {
                AuthenticatedClient = new AuthenticatedClient("")
                {
                    UserInformation = new UserInformation { UserName = "test_user" }
                }
            };

            //no such user
            A.CallTo(() => fakeBookmarksContext.GetUserByUsername("test_user")).Returns(null);
            //using fake context
            var sut = new SocialAuthenticationCallbackProvider(fakeBookmarksContext);
            var actual = (string)sut.Process(TestingModule.actualModule, callbackData);
            //now fail
            Assert.True(actual.StartsWith("login failed: "));            
        }

        [Fact]
        public void Should_get_token4_test_user()
        {
            var expectedUsername = "test_user_12345678@yahoo.de";
            var tokenService = new TokenService();
            string token = tokenService.GetToken(expectedUsername);
            var user = tokenService.GetUserFromToken(token);
            Assert.Equal(expectedUsername, user.UserName);
        }
    }

    public class TestingModule : NancyModule
    {
        public static IUserIdentity actualUser;
        public static TestingModule actualModule;

        public TestingModule()
        {
            Get["/testing"] = _ =>
            {
                actualUser = Context.CurrentUser;
                actualModule = this;
                return HttpStatusCode.OK;
            };
        }
    }
}
