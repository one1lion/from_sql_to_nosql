using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.CosmosModels.PlantCollection
{
    public class Plant
    {
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Acronym { get; set; }
        public IEnumerable<IContactInfo> ContactInfo { get; set; }
    }
}
