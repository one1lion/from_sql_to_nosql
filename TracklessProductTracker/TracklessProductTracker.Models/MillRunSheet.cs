using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class MillRunSheet
    {
        /// <summary>
        /// The database Autonumber Identity
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The internal Mill Run Sheet's Id
        /// </summary>
        public string MillRunSheetId { get; set; }
        /// <summary>
        /// The Tank Id
        /// </summary>
        public int TankId { get; set; }
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

        /// <summary>
        /// The Samples drawn on the Mill Run Sheet
        /// </summary>
        public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
        public virtual Tank Tank { get; set; }
    }
}
