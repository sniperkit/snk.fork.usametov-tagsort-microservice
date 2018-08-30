/*
Sniperkit-Bot
- Status: analyzed
*/

﻿using Xunit;

namespace TagSortService.Tests
{
    public class ConnectionStringTest
    {
        [Fact]
        public void Should_get_appHarbor_connection_string()
        {
            Assert.True(Utils.GetConnectionString().StartsWith("mongodb://"));            
        }

        [Fact]
        public void Should_get_twitter_key()
        {
            var key = Utils.GetAppSetting("TwitterConsumerKey");
            var secret = Utils.GetAppSetting("TwitterConsumerSecret");
            Assert.True(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(secret));
        }

        [Fact]
        public void Should_get_google_key()
        {
            var key = Utils.GetAppSetting("GoogleConsumerKey");
            var secret = Utils.GetAppSetting("GoogleConsumerSecret");
            Assert.True(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(secret));
        }

        [Fact]
        public void Should_get_facebook_key()
        {
            var key = Utils.GetAppSetting("FBClientId");
            var secret = Utils.GetAppSetting("FBClientSecret");
            Assert.True(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(secret));
        }
    }
}
