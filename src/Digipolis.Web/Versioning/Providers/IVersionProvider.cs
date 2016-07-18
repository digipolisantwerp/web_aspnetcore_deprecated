using System;

namespace Digipolis.Web.Versioning
{
    public interface IVersionProvider
    {
        AppVersion GetCurrentVersion();
    }
}
