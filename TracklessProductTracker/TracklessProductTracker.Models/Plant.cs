using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }

        public virtual ICollection<TrackingTicket> TrackingTickets { get; set; } = new List<TrackingTicket>();
    }
}
