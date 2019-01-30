using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Api;
using Digipolis.Web.SampleApi.Configuration;
using Digipolis.Web.SampleApi.Logic;
using Digipolis.Web.SampleApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Digipolis.Web.Api.Models;
using Digipolis.Web.Swagger;
using Microsoft.Extensions.Logging;

namespace Digipolis.Web.SampleApi.Controllers
{
    [Route("[controller]", Name = "Values")]
    public class ValuesController : Controller
    {
        private readonly IValueLogic _valueLogic;
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(IValueLogic valueLogic, ILogger<ValuesController> logger)
        {
            _valueLogic = valueLogic;
            _logger = logger;
        }

        /// <summary>
        /// Get all values 
        /// </summary>
        /// <remarks>
        /// This endpoint demostrates the use of the old <see cref="PagedResult{T}"/> and it's resulting json.
        /// </remarks>
        /// <param name="queryOptions">Query options from uri</param>
        /// <returns>An array of value objects</returns>
        [HttpGet()]
        [ProducesResponseType(typeof(PagedResult<ValueDto>), 200)]
        [AllowAnonymous]
        [Versions(Versions.V1, Versions.V2)]
        [Produces("application/hal+json")]
        public IActionResult Get([FromQuery]CriteriaDto criteria)
        {
            int total;
            var values = _valueLogic.GetAll(criteria, out total);
            //var result = queryOptions.ToPagedResult(values, total, "kevin", new { test = 0 });
            var result = criteria.ToPagedResult(values, total, "Get", "Values", new { test = 0 });
            return Ok(result);
        }

        /// <summary>
        /// Get all values 
        /// </summary>
        /// <remarks>
        /// This endpoint demostrates the use of the new <see cref="PagedResult{T, EmbeddedT}"/> and it's resulting json.
        /// </remarks>
        /// <param name="queryOptions">Query options from uri</param>
        /// <returns>An array of value objects</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(PagedResult<ValueDto, EmbeddedValueDto>), 200)]
        [AllowAnonymous]
        [Versions(Versions.V1, Versions.V2)]
        [Produces("application/hal+json")]
        public IActionResult GetNew([FromQuery]CriteriaDto criteria)
        {
            int total;
            var values = _valueLogic.GetAll(criteria, out total);
            //var result = queryOptions.ToPagedResult(values, total, "kevin", new { test = 0 });
            var result = criteria.ToPagedResult<ValueDto, EmbeddedValueDto>(values, total);
            return Ok(result);
        }
        /// <summary>
        /// Get a value by id
        /// </summary>
        /// <param name="id">The id of the value</param>
        /// <returns>A value object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ValueDto), 200)]
        [ProducesResponseType(typeof(ValueDto), 401)]
        [AllowAnonymous]
        [Produces("application/json", "text/csv")]
        public IActionResult Get(int id)
        {
            var value = _valueLogic.GetById(id);
            return Ok(value);
        }

        /// <summary>
        /// Add a new value
        /// </summary>
        /// <param name="value">A value object</param>
        /// <returns>The created value object</returns>
        [HttpPost]
        [ValidateModelState]
        [ProducesResponseType(typeof(ValueDto), 201)]
        [AllowAnonymous]
        [Versions(Versions.V1, Versions.V2)]
        public IActionResult Post([FromBody, Required] ValueDto value)
        {
            value = _valueLogic.Add(value);
            return CreatedAtAction("Get", new { id = value.Id }, value);
        }

        /// <summary>
        /// Update an existing value object
        /// </summary>
        /// <param name="id">The id of the value</param>
        /// <param name="value">The updated value object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ValidateModelState]
        [Versions(Versions.V1, Versions.V2)]
        [ExcludeSwaggerResonse((int)HttpStatusCode.NotFound)]
        public IActionResult Put(int id, [FromBody, Required] ValueDto value)
        {
            value = _valueLogic.Update(id, value);
            return Ok(value);
        }

        /// <summary>
        /// Delete a value by it's Id
        /// </summary>
        /// <param name="id">The value's Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Versions(Versions.V2)]
        public IActionResult Delete(int id)
        {
            _valueLogic.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Thorws an exception
        /// </summary>
        /// <returns></returns>
        [HttpGet("exception")]
        [AllowAnonymous]
        [Versions(Versions.V1, Versions.V2)]
        public IActionResult ThrowException()
        {
            throw new NotFoundException();
        }
    }
}
