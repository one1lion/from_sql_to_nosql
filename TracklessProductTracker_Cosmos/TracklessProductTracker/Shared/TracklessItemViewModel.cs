using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Shared
{
    public class TracklessItemViewModel
    {
        #region Top Level Information
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Category { get; set; }
        #endregion

        #region Shared Fields
        public string Name { get; set; }
        #endregion

        #region Chemical-specific fields
        public string CommonName { get; set; }
        #endregion

        #region Instrument-specific fields
        public string Instructions { get; set; }
        public string Comments { get; set; }
        #endregion

        #region Sample-specific fields
        public string SampleRetreivedBy { get; set; }
        public DateTime SampleDate { get; set; }
        public int? ContainerTypeId { get; set; }
        public string ContainerTypeName { get; set; }
        public int? MillRunSheetId { get; set; }
        #endregion
    }
}
