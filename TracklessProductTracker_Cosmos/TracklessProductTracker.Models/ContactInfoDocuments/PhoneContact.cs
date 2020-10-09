namespace TracklessProductTracker.Models
{
    public partial class PhoneContact : IContactInfo
    {
        public string Id { get; set; }
        public string Type { get; set; } = typeof(PhoneContact).FullName;
        public string PlantId { get; set; }
        public string PersonId { get; set; }
        public string PersonPlantId { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneType { get; set; }
        public bool PreferredContactMethod { get; set; }
    }
}
