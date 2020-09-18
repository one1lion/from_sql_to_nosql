using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class Instrument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TrackingTicketId { get; set; }
        // TODO: Describe other fields for Instrument item

        public TrackingTicket TrackingTicket { get; set; }
    }
}
