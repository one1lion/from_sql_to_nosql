using System;

namespace TracklessProductTracker.Shared
{
    public class TracklessItemViewModel
    {
        #region Top Level Information
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        #endregion

        #region Shared Fields
        public string Name { get; set; }
        #endregion

        #region Chemical-specific fields
        // TODO: Nothing here, yet
        #endregion

        #region Instrument-specific fields
        // TODO: Nothing here, yet
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