using System;
using System.Collections.Generic;
using System.Text;

namespace TracklessProductTracker.Shared
{
    public class TracklessItemViewModel
    {
        #region Top Level Information
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
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
        public DateTime? SampleDate { get; set; }
        public string ContainerTypeId { get; set; }
        public string ContainerTypeName { get; set; }
        public string MillRunSheetId { get; set; }
        #endregion
    }
}
