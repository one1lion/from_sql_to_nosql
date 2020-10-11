using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TracklessProductTracker.Models;

namespace TracklessProductTracker.Shared
{
    public class TrackingTicketViewModel
    {
        public string Id { get; set; }
        public Guid QrCodeGuid { get; set; }
        public string PlantId { get; set; }
        public string ItemId => Item?.Id;
        public string ItemCategoryId => Item?.CategoryId;
        public string ItemCategoryName => string.IsNullOrWhiteSpace(Item?.CategoryId)
            ? default
            : ItemCategories.SingleOrDefault(ic => ic.Id == Item?.CategoryId)?.Name;

        public string TechName { get; set; }
        public string ProductName { get; set; }
        public string FormulationDescriptionType { get; set; }
        public bool NewTicket { get; set; }

        public List<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
        public TracklessItemViewModel Item { get; set; } = new TracklessItemViewModel();
        public PlantViewModel Plant { get; set; } = new PlantViewModel();
    }
}
