using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class Chemical
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TrackingTicketId { get; set; }
        // TODO: Describe other fields for Chemical item

        public TrackingTicket TrackingTicket { get; set; }
    }
}
