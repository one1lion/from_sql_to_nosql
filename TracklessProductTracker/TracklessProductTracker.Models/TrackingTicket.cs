using System;
using System.Collections.Generic;

namespace TracklessProductTracker.Models
{
    public class TrackingTicket
    {

        public int Id { get; set; }
        /// <summary>
        /// The Guid to use on QrCodes (will be appended to the application's /track page's Uri)
        /// </summary>
        public Guid QrCodeGuid { get; set; }
        public int ItemCategoryId { get; set; }
        public int PlantId { get; set; }
        public string TechName { get; set; }
        public string ProductName { get; set; }
        public string FormulationDescriptionType { get; set; }
        public int? ChemicalId { get; set; }
        public int? InstrumentId { get; set; }
        public int? SampleId { get; set; }

        public virtual Chemical Chemical { get; set; }
        public virtual Instrument Instrument { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual Sample Sample { get; set; }
    }
}
