namespace TracklessProductTracker.Models
{
    public partial class EmailContact : IContactInfo
    {
        public string Id { get; set; }
        public string Type { get; set; } = typeof(EmailContact).FullName;
        public string PlantId { get; set; }
        public string PersonId { get; set; }
        public string PersonPlantId { get; set; }
        public string Email { get; set; }
        public bool PreferredContactMethod { get; set; }
    }
}
