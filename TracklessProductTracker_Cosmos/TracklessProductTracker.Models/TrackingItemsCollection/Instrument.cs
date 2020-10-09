using System;

namespace TracklessProductTracker.Models
{
    public partial class Instrument : ITrackingItem
    {
        public string Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = typeof(Instrument).FullName;
        public string TrackingTicketId { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public string Comments { get; set; }
    }
}
