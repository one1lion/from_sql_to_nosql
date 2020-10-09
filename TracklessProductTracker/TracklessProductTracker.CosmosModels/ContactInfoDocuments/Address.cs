using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.CosmosModels
{
    public class Address : IContactInfo
    {
        public string Type { get; set; } = typeof(Address).FullName;
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State_Province { get; set; }
        public string Zip { get; set; }
    }
}
