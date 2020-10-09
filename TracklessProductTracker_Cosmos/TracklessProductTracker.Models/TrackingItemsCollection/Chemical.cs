using System;

namespace TracklessProductTracker.Models
{
    public partial class Chemical : ITrackingItem
    {
        public string Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = typeof(Chemical).FullName;
        public string TrackingTicketId { get; set; }
        public string Name { get; set; }
        public string CommonName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
