using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.CosmosModels
{
    public class EmailContact : IContactInfo
    {
        public string Type { get; set; } = typeof(EmailContact).FullName;
        public string Email { get; set; }
        public bool PreferredContactMethod { get; set; }
    }
}
