using System;
using Digipolis.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Digipolis.Errors;

namespace Digipolis.Web.Monitoring
{
    //[Authorize(ActiveAuthenticationSchemes = AuthSchemes.JwtHeaderAuth)]
    [Route("[controller]")]
    public class StatusController : Controller
    {

        private readonly IStatusProvider _statusreader;
        private readonly ILogger<StatusController> _logger;

        public StatusController(IStatusProvider statusProvider, ILogger<StatusController> logger)
        {
            if (statusProvider == null) throw new ArgumentException( $"StatusController.Ctr parameter {nameof(statusProvider)} cannot be null. Register an IStatusProvider in the DI Container.");
            if (statusProvider == null) throw new ArgumentException($"StatusController.Ctr parameter {nameof(logger)} cannot be null.");

            _statusreader = statusProvider;
            _logger = logger;
        }

        /// <summary>
        /// Get the global API status and the components statusses.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Monitoring), 200)]
        [ProducesResponseType(typeof(Error), 500)]
        [Route("monitoring")]
        public async Task<IActionResult> GetMonitoring()
        {
            var status = await _statusreader.GetStatus();

            return Ok(status);
        }


        /// <summary>
        /// Get the global API status.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StatusResponse), 200)]
        [ProducesResponseType(typeof(Error), 500)]
        [Route("ping")]
        public async Task<IActionResult> GetPing()
        {
            return Ok(new StatusResponse()
            {
                Status = Status.ok
            });
        }


    }
}
