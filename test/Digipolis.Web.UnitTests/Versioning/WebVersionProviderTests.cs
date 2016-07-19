using System;
using Digipolis.Web.Versioning;
using Xunit;

namespace Digipolis.Web.UnitTests.Versioning
{
    public class WebVersionProviderTests
    {
        [Theory]
        [InlineData("1.2.3-5", "1")]
        [InlineData("1.2.3-", "1")]
        [InlineData("1.2.3", "1")]
        [InlineData("1", "1")]
        [InlineData("1-5", "1")]
        [InlineData(".2.3", "")]
        [InlineData("", "")]
        public void MajorVersion_Is_Parsed_Correctly(string version, string major)
        {

            var appEnv = new WebApplicationEnvironment() { ApplicationVersion = version };

            var provider = new WebVersionProvider(appEnv);

            Assert.Equal(major, provider.GetCurrentVersion().MajorVersion);
        }

        [Theory]
        [InlineData("1.2.3", "2")]
        [InlineData("1.2.3-6", "2")]
        [InlineData("1.2.3-", "2")]
        [InlineData("1..3", "")]
        [InlineData("1", "?")]
        [InlineData("1-5", "?")]
        [InlineData("1-", "?")]
        [InlineData("1.", "")]
        [InlineData("", "?")]
        public void MinorVersion_Is_Parsed_Correctly(string version, string minor)
        {
            var appEnv = new WebApplicationEnvironment() { ApplicationVersion = version };

            var provider = new WebVersionProvider(appEnv);

            Assert.Equal(minor, provider.GetCurrentVersion().MinorVersion);
        }

        [Theory]
        [InlineData("1.2.3", "3")]
        [InlineData("1.2.3-666", "3")]
        [InlineData("1.2.3-", "3")]
        [InlineData("1..4", "4")]
        [InlineData("..9", "9")]
        [InlineData("1", "?")]
        [InlineData("1.", "?")]
        [InlineData("1.2", "?")]
        [InlineData("1.2.", "")]
        [InlineData("", "?")]
        public void RevisionVersion_Is_Parsed_Correctly(string version, string revision)
        {
            var appEnv = new WebApplicationEnvironment() { ApplicationVersion = version };

            var provider = new WebVersionProvider(appEnv);

            Assert.Equal(revision, provider.GetCurrentVersion().Revision);
        }

        [Theory]
        [InlineData("1.2.3", "?")]
        [InlineData("1.2.3-666", "666")]
        [InlineData("1.2.3-", "")]
        [InlineData("1..4-5", "5")]
        [InlineData("..9-3", "3")]
        [InlineData("1", "?")]
        [InlineData("1.", "?")]
        [InlineData("1.2", "?")]
        [InlineData("1.2.", "?")]
        [InlineData("", "?")]
        public void Buildnumber_Is_Parsed_Correctly(string version, string buildnr)
        {
            var appEnv = new WebApplicationEnvironment() {  ApplicationVersion = version };

            var provider = new WebVersionProvider(appEnv);

            Assert.Equal(buildnr, provider.GetCurrentVersion().BuildNumber);
        }


        [Fact]
        public void Version_Equals_null_Is_Parsed_Correctly()
        {
            var appEnv = new WebApplicationEnvironment() { ApplicationVersion = null };

            var provider = new WebVersionProvider(appEnv);

            Assert.Equal("", provider.GetCurrentVersion().MajorVersion);
        }

        [Fact]
        public void ApplicationBasePath_Equals_null_Is_Parsed_Correctly()
        {
            var appEnv = new WebApplicationEnvironment() { ApplicationBasePath = null };

            var provider = new WebVersionProvider(appEnv);

            Assert.Equal("?", provider.GetCurrentVersion().BuildDate);
        }

    }
}
