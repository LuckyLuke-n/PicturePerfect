﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public class Location
    {
        /// <summary>
        /// Get the location id.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Get or set the location name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the notex for this location.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Creates a new instance of the location class.
        /// </summary>
        public Location()
        {

        }
    }
}
