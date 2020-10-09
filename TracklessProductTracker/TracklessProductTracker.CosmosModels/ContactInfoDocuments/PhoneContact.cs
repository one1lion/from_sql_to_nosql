using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.CosmosModels
{
    public class PhoneContact : IContactInfo
    {
        public string Type { get; set; } = typeof(PhoneContact).FullName;
        public string PhoneNumber { get; set; }
        public bool PreferredContactMethod { get; set; }
    }
}
