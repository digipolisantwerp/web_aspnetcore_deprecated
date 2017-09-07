using System;
using Microsoft.Extensions.Options;
using Digipolis.Web.Monitoring;

namespace Digipolis.Web.UnitTests
{
    public class TestMonitoringOptions : IOptions<MonitoringOptions>
    {
        public TestMonitoringOptions(MonitoringOptions options)
        {
            Value = options;
        }

        public MonitoringOptions Value { get; private set; }
    }
}
