using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class Tank
    {
        /// <summary>
        /// The database Autonumber Identity
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The internal TankId
        /// </summary>
        public string TankId { get; set; }
        /// <summary>
        /// The current number of Gallons in the tank
        /// </summary>
        public int CurrentGallonsInTank { get; set; } = 0;

        public ICollection<MillRunSheet> MillRunSheets { get; set; } = new List<MillRunSheet>();
    }
}
