namespace TracklessProductTracker.Models
{
    public partial class ItemCategory
    {
        public int Id { get; set; }
        public string Type { get; set; } = typeof(ItemCategory).FullName;
        public string Name { get; set; }
    }
}
