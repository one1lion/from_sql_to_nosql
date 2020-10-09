using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TracklessProductTracker.Database;
using TracklessProductTracker.Models;
using TracklessProductTracker.Shared;

namespace TracklessProductTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TracklessProductContext context;

        public TicketsController(TracklessProductContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<TrackingTicketViewModel> GetTrackingTickets()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{qrCodeGuid}")]
        public TrackingTicketViewModel GetTrackingTicket(string qrCodeGuid)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<TrackingTicket> CreateTrackingTicket(TrackingTicket qcTicket)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<TrackingTicket> UpdateTrackingTicket(TrackingTicket qcTicket)
        {
            throw new NotImplementedException();
        }

    }
}
