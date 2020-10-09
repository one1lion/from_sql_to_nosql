using System;

namespace TracklessProductTracker.Models
{
    public partial class TrackingTicket
    {
        public string Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public int PlantId { get; set; }
        public string TechName { get; set; }
        public string ItemId { get; set; }
    }
}
