using System.Collections.Generic;

namespace TracklessProductTracker.Shared
{
    public class SaveTicketResponseModel
    {
        public TrackingTicketViewModel ReturnItem { get; set; }
        public bool WasError { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
