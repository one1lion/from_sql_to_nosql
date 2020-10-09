namespace TracklessProductTracker.Models
{
    public partial class PhoneContact : IContactInfo
    {
        public string Type { get; set; } = typeof(PhoneContact).FullName;
        public string PhoneNumber { get; set; }
        public string PhoneType { get; set; }
        public bool PreferredContactMethod { get; set; }
    }
}
