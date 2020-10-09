using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class MillRunSheet
    {
        public string Id { get; set; }
        /// <summary>
        /// The internal Mill Run Sheet's Id
        /// </summary>
        public string MillRunSheetId { get; set; }
        /// <summary>
        /// The Tank Id
        /// </summary>
        public string TankId { get; set; }
        /// <summary>
        /// The internal Batch Id
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// The Lot the Mill Run Sheet is for
        /// </summary>
        public string Lot { get; set; }
        /// <summary>
        /// The record creation's Date/Time Stamp
        /// </summary>
        public DateTime DateTimeStamp { get; set; } = DateTime.UtcNow;

        // TODO: Array of Sample Ids?
        //public IEnumerable<int> SampleId { get; set; }
    }
}
