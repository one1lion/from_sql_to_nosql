using System;
using System.Collections.Generic;

namespace TracklessProductTracker.Models
{
    public partial class Sample : ITrackingItem
    {
        public string Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = typeof(Sample).FullName;
        public string TrackingTicketId { get; set; }
        public string SampleRetreivedBy { get; set; }
        public DateTime SampleDate { get; set; }
        public string ContainerTypeName { get; set; }
        public string MillRunSheetId { get; set; }
        public string ContainerTypeId { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public IEnumerable<Test> Tests { get; set; }
        public DateTime DateTimeStamp { get; set; } = DateTime.UtcNow;
    }
}
