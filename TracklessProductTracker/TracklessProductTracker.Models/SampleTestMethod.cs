using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class SampleTestMethod
    {
        public int Id { get; set; }
        public int SampleTestId { get; set; }
        public int TestMethodId { get; set; }

        public virtual SampleTest SampleTest { get; set; }
        public virtual TestMethod TestMethod { get; set; }
    }
}
