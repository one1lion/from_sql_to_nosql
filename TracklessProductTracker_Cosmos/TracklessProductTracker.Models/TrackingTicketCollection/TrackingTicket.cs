using System;

namespace TracklessProductTracker.Models
{
    public partial class TrackingTicket
    {
        public string Id { get; set; }
        public Guid QrCodeGuid { get; set; } 
        public string PlantId { get; set; }
        public string PlantName { get; set; }
        public string PlantAcronym { get; set; }
        public string ItemId { get; set; }
        public string ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public string TechName { get; set; }
        public string ProductName { get; set; }
        public string FormulationDescriptionType { get; set; }
    }
}
