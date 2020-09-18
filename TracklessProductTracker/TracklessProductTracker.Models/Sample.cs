using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class Sample
    {
        public int Id { get; set; }
        public string SampleRetreivedBy { get; set; }
        public DateTime SampleDate { get; set; }
        public int? ContainerTypeId { get; set; }
        public int? TrackingTicketId { get; set; }
        public int MillRunSheetId { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public virtual ContainerType ContainerType { get; set; }
        public virtual TrackingTicket TrackingTicket { get; set; }
        public virtual MillRunSheet MillRunSheet { get; set; }
        public virtual ICollection<SampleTest> SampleTests { get; set; } = new List<SampleTest>();
    }
}
