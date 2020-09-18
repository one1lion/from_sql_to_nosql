using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TracklessProductTracker.Models;

namespace TracklessProductTracker.Shared
{
    public class TrackingTicketViewModel
    {
        public int? Id { get; set; }
        public Guid QrCodeGuid { get; set; }
        public int? ItemCategoryId { get; set; }
        private string _ItemCategoryName;
        public string ItemCategoryName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ItemCategoryName)) {
                    return !ItemCategoryId.HasValue ? default : ItemCategories.SingleOrDefault(ic => ic.Id == ItemCategoryId)?.Name;
                }
                return _ItemCategoryName;
            }
            set
            {
                _ItemCategoryName = value;
            }
        }
        public int? ItemId { get; set; }
        public string TechName { get; set; }
        public string ProductName { get; set; }
        public string FormulationDescriptionType { get; set; }

        public List<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();

        public TracklessItemViewModel Item { get; set; } = new TracklessItemViewModel();
    }
}
