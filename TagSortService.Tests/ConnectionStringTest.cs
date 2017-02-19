using Xunit;

namespace TagSortService.Tests
{
    public class ConnectionStringTest
    {
        [Fact]
        public void Should_get_appHarbor_connection_string()
        {
            Assert.Equal(Utils.GetConnectionString(), "test_connect_string");
        }
    }
}
