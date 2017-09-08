using Digipolis.Web.Monitoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Controllers
{
    public class StatusControllerTest
    {
        [Fact]
        public void CtrThrowsExceptionIfIStatusProviderIsNull()
        {
            var logger = new Moq.Mock<ILogger<StatusController>>().Object;

            Assert.Throws<ArgumentException>(() => new StatusController(null, logger));
        }

        [Fact]
        public void CtrThrowsExceptionIfLoggerIsNull()
        {
            var statusProvider = new Moq.Mock<IStatusProvider>().Object;

            Assert.Throws<ArgumentException>(() => new StatusController(statusProvider, null));
        }

        [Fact]
        public async Task GetStatusUsesIStatusProvider()
        {
            var logger = new Moq.Mock<ILogger<StatusController>>().Object;
            var statusProviderMock = new Moq.Mock<IStatusProvider>();
            statusProviderMock.Setup(x => x.GetStatus()).Returns(Task.FromResult(new Monitoring.Monitoring() { Status = Status.warning })).Verifiable();

            var controller = new StatusController(statusProviderMock.Object, logger);

            var result = (Monitoring.Monitoring)(await controller.GetMonitoring() as OkObjectResult).Value;

            statusProviderMock.Verify(x => x.GetStatus(),Times.Once());
            Assert.Equal(Status.warning, result.Status);
        }
    }
}
