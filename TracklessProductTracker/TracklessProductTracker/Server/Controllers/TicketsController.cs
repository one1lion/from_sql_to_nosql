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
        public List<TrackingTicketViewModel> GetTrackingTickets()
        {
            // TODO: Limit to page size and get page
            return context.TrackingTickets
                .Include(e => e.ItemCategory)
                .Include(e => e.Chemical)
                .Include(e => e.Instrument)
                .Include(e => e.Sample)
                .Select(e => new TrackingTicketViewModel()
                {
                    Id = e.Id,
                    QrCodeGuid = e.QrCodeGuid,
                    FormulationDescriptionType = e.FormulationDescriptionType,
                    ItemCategoryId = e.ItemCategoryId,
                    ItemCategoryName = e.ItemCategory == null ? default : e.ItemCategory.Name,
                    ItemId = e.ItemCategory == null ? default :
                        e.ItemCategory.Name == "Chemical" ? e.ChemicalId :
                        e.ItemCategory.Name == "Instrument" ? e.InstrumentId :
                        e.ItemCategory.Name == "Sample" ? e.SampleId : default,
                    ProductName = e.ProductName,
                    TechName = e.TechName,
                    Item = e.ItemCategory == null ? new TracklessItemViewModel() :
                        e.ItemCategory.Name == "Chemical" && e.Chemical != null ?
                            new TracklessItemViewModel()
                            {
                                Id = e.Chemical.Id,
                                Name = e.Chemical.Name
                            } :
                        e.ItemCategory.Name == "Instrument" && e.Instrument != null ?
                            new TracklessItemViewModel()
                            {
                                Id = e.Instrument.Id,
                                Name = e.Instrument.Name
                            } :
                        e.ItemCategory.Name == "Sample" && e.Sample != null ?
                            new TracklessItemViewModel()
                            {
                                Id = e.Sample.Id,
                                SampleRetreivedBy = e.Sample.SampleRetreivedBy,
                                SampleDate = e.Sample.SampleDate,
                                ContainerTypeId = e.Sample.ContainerTypeId,
                                ContainerTypeName = e.Sample.ContainerType == null ? default : e.Sample.ContainerType.Name,
                                MillRunSheetId = e.Sample.MillRunSheetId
                            }
                        : new TracklessItemViewModel()
                }).ToList();
        }

        [HttpGet("{qrCodeGuid}")]
        public TrackingTicketViewModel GetTrackingTicket(string qrCodeGuid)
        {
            var itemCats = context.ItemCategories.OrderBy(e => e.Name).ToList();
            if (!Guid.TryParse(qrCodeGuid, out Guid qrCode))
            {
                return new TrackingTicketViewModel()
                {
                    ItemCategories = itemCats
                };
            }
            return context.TrackingTickets
                .Include(e => e.ItemCategory)
                .Include(e => e.Chemical)
                .Include(e => e.Instrument)
                .Include(e => e.Sample)
                .Where(e => e.QrCodeGuid == qrCode)
                .Select(e => new TrackingTicketViewModel()
                {
                    Id = e.Id,
                    QrCodeGuid = e.QrCodeGuid,
                    FormulationDescriptionType = e.FormulationDescriptionType,
                    ItemCategoryId = e.ItemCategoryId,
                    ItemCategoryName = e.ItemCategory == null ? default : e.ItemCategory.Name,
                    ItemId = e.ItemCategory == null ? default :
                        e.ItemCategory.Name == "Chemical" ? e.ChemicalId :
                        e.ItemCategory.Name == "Instrument" ? e.InstrumentId :
                        e.ItemCategory.Name == "Sample" ? e.SampleId : default,
                    ProductName = e.ProductName,
                    TechName = e.TechName,
                    ItemCategories = itemCats,
                    Item = e.ItemCategory == null ? new TracklessItemViewModel() :
                        e.ItemCategory.Name == "Chemical" && e.Chemical != null ?
                            new TracklessItemViewModel()
                            {
                                Id = e.Chemical.Id,
                                Name = e.Chemical.Name
                            } :
                        e.ItemCategory.Name == "Instrument" && e.Instrument != null ?
                            new TracklessItemViewModel()
                            {
                                Id = e.Instrument.Id,
                                Name = e.Instrument.Name
                            } :
                        e.ItemCategory.Name == "Sample" && e.Sample != null ?
                            new TracklessItemViewModel()
                            {
                                Id = e.Sample.Id,
                                SampleRetreivedBy = e.Sample.SampleRetreivedBy,
                                SampleDate = e.Sample.SampleDate,
                                ContainerTypeId = e.Sample.ContainerTypeId,
                                ContainerTypeName = e.Sample.ContainerType == null ? default : e.Sample.ContainerType.Name,
                                MillRunSheetId = e.Sample.MillRunSheetId
                            }
                        : new TracklessItemViewModel()
                }).SingleOrDefault() ?? new TrackingTicketViewModel()
                {
                    QrCodeGuid = qrCode,
                    ItemCategories = itemCats
                };
        }

        [HttpPost]
        public async Task<TrackingTicket> CreateTrackingTicket(TrackingTicket qcTicket)
        {
            var newIdSet = false;
            using var trans = await context.Database.BeginTransactionAsync();
            try
            {
                if (qcTicket.QrCodeGuid == default)
                {
                    qcTicket.QrCodeGuid = Guid.NewGuid();
                    newIdSet = true;
                }
                context.TrackingTickets.Add(qcTicket);
                await context.SaveChangesAsync();
                await trans.CommitAsync();
                return qcTicket;
            }
            catch (Exception)
            {
                if (trans is { }) { await trans.RollbackAsync(); }
                if (newIdSet) { qcTicket.QrCodeGuid = default; }
                throw;
            }
        }

        [HttpPut]
        public async Task<TrackingTicket> UpdateTrackingTicket(TrackingTicket qcTicket)
        {
            using var trans = await context.Database.BeginTransactionAsync();
            try
            {
                context.Entry(qcTicket).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await trans.CommitAsync();
                return qcTicket;
            }
            catch (Exception)
            {
                if (trans is { }) { await trans.RollbackAsync(); }
                throw;
            }
        }
    }
}
