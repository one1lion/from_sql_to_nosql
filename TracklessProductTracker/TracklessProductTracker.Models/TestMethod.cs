using System.Collections.Generic;

namespace TracklessProductTracker.Models
{
    public partial class TestMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // TODO: Possibly have a related entity for allowed on tests
        // TODO: Possibly have a related entity for allowed instruments
        public virtual ICollection<SampleTestMethod> SampleTestMethods { get; set; } = new List<SampleTestMethod>();
    }
}
