using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<TrackingTicketViewModel>> GetTrackingTickets()
        {
            var itemCats = await context.ItemCategories.ToListAsync();
            var items = await context.Chemicals.Select(e => new TracklessItemViewModel()
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                CategoryName = e.CategoryName,
                Name = e.Name,
                CommonName = e.CommonName
            }).ToListAsync();
            items = items.Union(await context.Instruments.Select(e => new TracklessItemViewModel()
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                CategoryName = e.CategoryName,
                Name = e.Name,
                Instructions = e.Instructions,
                Comments = e.Comments
            }).ToListAsync()).ToList();
            items = items.Union(await context.Samples.Select(e => new TracklessItemViewModel()
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                CategoryName = e.CategoryName,
                SampleRetreivedBy = e.SampleRetreivedBy,
                SampleDate = e.SampleDate,
                ContainerTypeId = e.ContainerTypeId,
                ContainerTypeName = e.ContainerTypeName,
                MillRunSheetId = e.MillRunSheetId,
            }).ToListAsync()).ToList();
            return await context.TrackingTickets.Select(e => new TrackingTicketViewModel()
            {
                Id = e.Id,
                QrCodeGuid = e.QrCodeGuid,
                ItemCategoryId = e.ItemCategoryId,
                ItemCategoryName = e.ItemCategoryName,
                ItemId = e.ItemId,
                TechName = e.TechName,
                ItemCategories = itemCats,
                Item = items.SingleOrDefault(i => i.Id == e.ItemId)
            }).ToListAsync();
        }

        [HttpGet("{qrCodeGuid}")]
        public async Task<TrackingTicketViewModel> GetTrackingTicket(string qrCodeGuid)
        {
            var itemCats = await context.ItemCategories.ToListAsync();
            if (!Guid.TryParse(qrCodeGuid, out Guid qrCode))
            {
                return new TrackingTicketViewModel()
                {
                    ItemCategories = itemCats
                };
            }

            TracklessItemViewModel item = null;
            var foundTicket = await context.TrackingTickets
                .Where(e => e.QrCodeGuid == qrCode)
                .Select(e => new TrackingTicketViewModel()
                {
                    Id = e.Id,
                    QrCodeGuid = e.QrCodeGuid,
                    ItemCategoryId = e.ItemCategoryId,
                    ItemCategoryName = e.ItemCategoryName,
                    ItemId = e.ItemId,
                    TechName = e.TechName,
                    ItemCategories = itemCats
                }).SingleOrDefaultAsync();
            if (foundTicket is { })
            {
                if (string.IsNullOrWhiteSpace(foundTicket.ItemCategoryName) || string.IsNullOrWhiteSpace(foundTicket.ItemId))
                {
                    item = new TracklessItemViewModel();
                }
                else
                {
                    item = foundTicket.ItemCategoryName switch
                    {
                        "Chemical" =>
                            await context.Chemicals.Select(e => new TracklessItemViewModel()
                            {
                                Id = e.Id,
                                CategoryId = e.CategoryId,
                                CategoryName = e.CategoryName,
                                Name = e.Name,
                                CommonName = e.CommonName
                            }).SingleOrDefaultAsync(e => e.Id == foundTicket.ItemId),
                        "Instrument" =>
                            await context.Instruments.Select(e => new TracklessItemViewModel()
                            {
                                Id = e.Id,
                                Name = e.Name
                            }).SingleOrDefaultAsync(e => e.Id == foundTicket.ItemId),

                        "Sample" =>
                            await context.Samples.Select(e => new TracklessItemViewModel()
                            {
                                Id = e.Id,
                                SampleRetreivedBy = e.SampleRetreivedBy,
                                SampleDate = e.SampleDate,
                                ContainerTypeId = e.ContainerTypeId,
                                ContainerTypeName = e.ContainerTypeName,
                                MillRunSheetId = e.MillRunSheetId
                            }).SingleOrDefaultAsync(e => e.Id == foundTicket.ItemId),
                        _ => new TracklessItemViewModel()
                    };
                }

                foundTicket.Item = item ?? new TracklessItemViewModel();
            }
            return foundTicket
                ?? new TrackingTicketViewModel()
                {
                    QrCodeGuid = qrCode,
                    ItemCategories = itemCats
                };
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
