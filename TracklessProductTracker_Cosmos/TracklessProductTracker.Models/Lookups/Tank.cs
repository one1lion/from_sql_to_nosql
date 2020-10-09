using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class Tank
    {
        public string Id { get; set; }
        public string Type { get; set; } = "Tank";
        /// <summary>
        /// The internal TankId
        /// </summary>
        public string TankId { get; set; }
        /// <summary>
        /// The plant this tank is located at
        /// </summary>
        public string PlantId { get; set; }
        /// <summary>
        /// The current number of Gallons in the tank
        /// </summary>
        public int CurrentGallonsInTank { get; set; } = 0;
    }
}
