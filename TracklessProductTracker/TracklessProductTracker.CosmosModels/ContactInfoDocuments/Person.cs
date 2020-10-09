using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.CosmosModels
{
    public class Person : IContactInfo
    {
        public string Type { get; set; } = typeof(Person).FullName;
        public string Role { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public IEnumerable<IContactInfo> Contact { get; set; }
    }
}
