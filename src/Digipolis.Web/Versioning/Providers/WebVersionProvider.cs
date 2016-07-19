using System;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace Digipolis.Web.Versioning
{
    public class WebVersionProvider : IVersionProvider
    {
        private IApplicationEnvironment _appEnv;

        public WebVersionProvider()
        {
            _appEnv = new WebApplicationEnvironment();
        }

        public WebVersionProvider(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        /// <summary>
        /// Retrieve the current version of the component
        /// </summary>
        /// <returns>AppVersion object</returns>
        public AppVersion GetCurrentVersion()
        {
            try
            {
                AppVersion retAV = new AppVersion()
                {
                    AppName = _appEnv.ApplicationName ?? ""
                };

                string[] splitbuildnr = (_appEnv.ApplicationVersion ?? "").Split('-');                    
                string[] splitversion = (splitbuildnr[0]).Split('.');

                retAV.MajorVersion = splitversion[0];
                retAV.MinorVersion = splitversion.Length > 1 ? splitversion[1] : "?";
                retAV.Revision = splitversion.Length > 2 ? splitversion[2] : "?";

                retAV.BuildNumber = splitbuildnr.Length > 1 ? splitbuildnr[1] : "?";


                string[] Paths = Directory.GetFiles(_appEnv.ApplicationBasePath ?? ".", "project.json");
               
                if (Paths.Length == 1)
                {
                    FileInfo versieFileInfo = new FileInfo(Paths[0]);
                    retAV.BuildDate = versieFileInfo.CreationTime.ToString();
                    if (_appEnv.ApplicationBasePath == null)
                    {
                        retAV.BuildDate = "?"; 
                    }                   
                }
                else
                {                    
                    retAV.BuildDate = "?";                   
                }

                return retAV;

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while determining the version data.", ex);
            }
        }

    }
}
