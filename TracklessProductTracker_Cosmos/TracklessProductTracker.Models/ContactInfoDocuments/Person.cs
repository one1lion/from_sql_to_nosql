﻿using System.Collections.Generic;

namespace TracklessProductTracker.Models
{
    public partial class Person : IContactInfo
    {
        public string Id { get; set; }
        public string Type { get; set; } = typeof(Person).FullName;
        public string PlantId { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public IEnumerable<EmailContact> EmailAddresses { get; set; }
        public IEnumerable<PhoneContact> PhoneNumbers { get; set; }
    }
}
