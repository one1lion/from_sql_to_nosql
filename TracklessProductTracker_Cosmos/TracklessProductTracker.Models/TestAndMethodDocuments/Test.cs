using System;
using System.Collections.Generic;

namespace TracklessProductTracker.Models
{
    public partial class Test
    {
        public int Sequence { get; set; }
        public string TestType { get; set; }
        public DateTime CompletedDate { get; set; }
        public string ResultsSummary { get; set; }
        public string Tester { get; set; }
        public IEnumerable<TestMethod> Methods { get; set; }
    }
}
