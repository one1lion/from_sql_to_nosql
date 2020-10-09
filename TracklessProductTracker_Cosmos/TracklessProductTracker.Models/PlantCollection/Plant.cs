using System;
using System.Collections.Generic;

namespace TracklessProductTracker.Models
{
    public partial class Plant
    {
        public string Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Acronym { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Person> ContactPeople { get; set; }
        public IEnumerable<EmailContact> EmailAddresses { get; set; }
        public IEnumerable<PhoneContact> PhoneNumbers { get; set; }

    }
}
