namespace TracklessProductTracker.Models
{
    public partial class EmailContact : IContactInfo
    {
        public string Type { get; set; } = typeof(EmailContact).FullName;
        public string Email { get; set; }
        public bool PreferredContactMethod { get; set; }
    }
}
