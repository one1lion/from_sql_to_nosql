using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Models
{
    public partial class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // TODO: Possibly have a related entity for allowed TestMethods
        public ICollection<SampleTest> SampleTests { get; set; } = new List<SampleTest>();
    }
}
