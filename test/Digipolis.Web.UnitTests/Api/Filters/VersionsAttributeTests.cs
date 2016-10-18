using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Api;
using Digipolis.Web.Api.Constraints;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Filters
{
    public class VersionsAttributeTests
    {
        [Fact]
        public void AcceptedVersionNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new VersionsAttribute(null));
        }

        [Fact]
        public void AcceptedVersionEmptyThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VersionsAttribute(new string[0]));
        }

        [Fact]
        public void AcceptedVersionEmptyStringThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new VersionsAttribute(new string[] { "" }));
        }

        [Fact]
        public void CreateInstanceReturnsVersionConstraint()
        {
            var versions = new VersionsAttribute(new string[] {"v1"});
            Assert.True(typeof(VersionConstraint) == versions.CreateInstance(null).GetType());
        }
    }
}
