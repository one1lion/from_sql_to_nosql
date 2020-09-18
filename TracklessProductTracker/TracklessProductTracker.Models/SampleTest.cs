using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class SampleTest
    {
        public int Id { get; set; }
        public int SampleId { get; set; }
        public int TestId { get; set; }
        public long? GallonsInTank { get; set; }
        public string AsphaltSupplier { get; set; }
        public string TestSampleType { get; set; }
        public string TestNumber { get; set; }
        public string Comments { get; set; }

        public virtual Sample Sample { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<SampleTestMethod> SampleTestMethods { get; set; } = new List<SampleTestMethod>();
    }
}
