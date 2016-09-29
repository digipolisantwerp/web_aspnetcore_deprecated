using System;

namespace Digipolis.Web
{
    public class AppVersion
    {

        public String AppName { get; set; }

        public String MajorVersion { get; set; }

        public String MinorVersion { get; set; }

        public String Revision { get; set; }

        public String BuildNumber { get; set; }

        public String BuildDate { get; set; }

        public String FullVersion => $"{MajorVersion}.{MinorVersion}.{Revision}.{BuildNumber}";
    }
}
