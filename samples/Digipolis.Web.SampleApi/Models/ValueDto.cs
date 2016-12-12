using Digipolis.Web.UnitTests.Modelbinders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Digipolis.Web.SampleApi.Models
{
    /// <summary>
    /// A value type
    /// </summary>
    public class ValueDto
    {
        /// <summary>
        /// Id of the value type
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique name to identify a value type
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The type of Value
        /// </summary>
        public ValueTypeDto ValueType { get; set; }

        /// <summary>
        /// Creation date and time
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Time since creation
        /// </summary>
        public TimeSpan TimeSinceCreation => DateTime.UtcNow.Subtract(CreationDate.ToUniversalTime());

        
        public string[] StringArray { get; set; }
    }
}
