using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class ItemCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TrackingTicket> TrackingTickets { get; set; } = new List<TrackingTicket>();
    }
}
