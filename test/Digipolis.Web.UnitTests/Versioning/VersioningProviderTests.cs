//using System;
//using Digipolis.Web.Versioning;
//using Moq;
//using Xunit;
//using Microsoft.Extensions.PlatformAbstractions;

//namespace Digipolis.Web.UnitTests.Versioning
//{
//    public class VersioningProviderTests
//    {
//        [Theory]
//        [InlineData("1.2.3-5", "1")]
//        [InlineData("1.2.3-", "1")]
//        [InlineData("1.2.3", "1")]
//        [InlineData("1", "1")]
//        [InlineData("1-5", "1")]
//        [InlineData(".2.3", "")]
//        [InlineData("", "")]        
//        public void MajorVersion_Is_Parsed_Correctly(string version, string major)
//        {

//            var appEnv = new ApplicationEnvironment();
            

//            var appenv_mock = new Mock<ApplicationEnvironment>();
//            appenv_mock.Setup(foo => foo.ApplicationVersion).Returns(version).Verifiable();
            
//            var provider = new WebVersionProvider(appenv_mock.Object);

//            Assert.Equal(major, provider.GetCurrentVersion().MajorVersion);
//            appenv_mock.Verify();
//        }

//        [Theory]
//        [InlineData("1.2.3", "2")]
//        [InlineData("1.2.3-6", "2")]
//        [InlineData("1.2.3-", "2")]
//        [InlineData("1..3", "")]
//        [InlineData("1", "?")]
//        [InlineData("1-5", "?")]
//        [InlineData("1-", "?")]
//        [InlineData("1.", "")]
//        [InlineData("", "?")]
//        public void MinorVersion_Is_Parsed_Correctly(string version, string minor)
//        {
//            var appenv_mock = new Mock<ApplicationEnvironment>();
//            appenv_mock.Setup(foo => foo.ApplicationVersion).Returns(version).Verifiable();

//            var provider = new WebVersionProvider(appenv_mock.Object);

//            Assert.Equal(minor, provider.GetCurrentVersion().MinorVersion);
//            appenv_mock.Verify();
//        }

//        [Theory]
//        [InlineData("1.2.3", "3")]
//        [InlineData("1.2.3-666", "3")]
//        [InlineData("1.2.3-", "3")]
//        [InlineData("1..4", "4")]
//        [InlineData("..9", "9")]
//        [InlineData("1", "?")]
//        [InlineData("1.", "?")]
//        [InlineData("1.2", "?")]
//        [InlineData("1.2.", "")]
//        [InlineData("", "?")]
//        public void RevisionVersion_Is_Parsed_Correctly(string version, string revision)
//        {
//            var appenv_mock = new Mock<ApplicationEnvironment>();
//            appenv_mock.Setup(foo => foo.ApplicationVersion).Returns(version).Verifiable();

//            var provider = new WebVersionProvider(appenv_mock.Object);

//            Assert.Equal(revision, provider.GetCurrentVersion().Revision);
//            appenv_mock.Verify();
//        }


//        [Theory]
//        [InlineData("1.2.3", "?")]
//        [InlineData("1.2.3-666", "666")]
//        [InlineData("1.2.3-", "")]
//        [InlineData("1..4-5", "5")]
//        [InlineData("..9-3", "3")]
//        [InlineData("1", "?")]
//        [InlineData("1.", "?")]
//        [InlineData("1.2", "?")]
//        [InlineData("1.2.", "?")]
//        [InlineData("", "?")]
//        public void Buildnumber_Is_Parsed_Correctly(string version, string buildnr)
//        {
//            var appenv_mock = new Mock<ApplicationEnvironment>();
//            appenv_mock.Setup(foo => foo.ApplicationVersion).Returns(version).Verifiable();

//            var provider = new WebVersionProvider(appenv_mock.Object);

//            Assert.Equal(buildnr, provider.GetCurrentVersion().BuildNumber);
//            appenv_mock.Verify();
//        }


//        [Fact]
//        public void Version_Equals_null_Is_Parsed_Correctly()
//        {
//            var appenv_mock = new Mock<ApplicationEnvironment>();
//            appenv_mock.Setup(foo => foo.ApplicationVersion).Verifiable();

//            var provider = new WebVersionProvider(appenv_mock.Object);

//            Assert.Equal("", provider.GetCurrentVersion().MajorVersion);
//            appenv_mock.Verify();
//        }
        
//        [Fact]
//        public void ApplicationBasePath_Equals_null_Is_Parsed_Correctly()
//        {
//            var appenv_mock = new Mock<ApplicationEnvironment>();
//            appenv_mock.Setup(foo => foo.ApplicationBasePath).Verifiable();

//            var provider = new WebVersionProvider(appenv_mock.Object);

//            Assert.Equal("?", provider.GetCurrentVersion().BuildDate);
//            //Assert.Equal("?", provider.GetCurrentVersion().BuildNumber);
//            appenv_mock.Verify();
//        }
        
//    }
//}
