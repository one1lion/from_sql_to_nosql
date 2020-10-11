using System;

namespace TracklessProductTracker.Models
{
    public interface ITrackingItem
    {
        string Id { get; set; }
        Guid Guid { get; set; }
        string TrackingTicketId { get; set; }
        string CategoryId { get; set; }
        string CategoryName { get; set; }
        string Type { get; set; }
    }
}
