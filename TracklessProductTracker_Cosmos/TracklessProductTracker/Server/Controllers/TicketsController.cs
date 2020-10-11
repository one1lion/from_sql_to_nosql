using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        public async Task<List<TrackingTicketViewModel>> GetTrackingTickets(bool withDetails = false, CancellationToken ct = default)
        {
            try
            {
                // Get the list of Tracking tickets
                // NOTE: This will need to be reworked to accomodate record count growth; that is,
                //       this should probably be done in a paged kind of way
                var foundTickets = await context.TrackingTickets.ToListAsync(ct);
                if ((foundTickets?.Count ?? 0) == 0) { return new List<TrackingTicketViewModel>(); }

                // Initialize the list of return items and item categories
                // It only gets items whose Ids are in the list of retrieved tickets' ItemIds
                var itemIds = foundTickets.Where(e => !string.IsNullOrWhiteSpace(e.ItemId))
                    .Select(e => e.ItemId).Distinct().ToList();
                List<TracklessItemViewModel> items = new List<TracklessItemViewModel>();
                List<ItemCategory> itemCats = new List<ItemCategory>();
                if (withDetails)
                {
                    // Since the request is for all tickets with details, this prepares to 
                    // items to be returned with the Ticket view model
                    itemCats = await context.ItemCategories.ToListAsync(ct);
                    items = await context.Chemicals.Where(e => itemIds.Contains(e.Id))
                        .Select(e => new TracklessItemViewModel()
                        {
                            Id = e.Id,
                            CategoryId = e.CategoryId,
                            CategoryName = e.CategoryName,
                            Name = e.Name,
                            CommonName = e.CommonName
                        }).ToListAsync(ct);
                    items = items.Union(await context.Instruments.Where(e => itemIds.Contains(e.Id))
                        .Select(e => new TracklessItemViewModel()
                        {
                            Id = e.Id,
                            CategoryId = e.CategoryId,
                            CategoryName = e.CategoryName,
                            Name = e.Name,
                            Instructions = e.Instructions,
                            Comments = e.Comments
                        }).ToListAsync(ct)).ToList();
                    items = items.Union(await context.Samples.Where(e => itemIds.Contains(e.Id))
                        .Select(e => new TracklessItemViewModel()
                        {
                            Id = e.Id,
                            CategoryId = e.CategoryId,
                            CategoryName = e.CategoryName,
                            SampleRetreivedBy = e.SampleRetreivedBy,
                            SampleDate = e.SampleDate,
                            ContainerTypeId = e.ContainerTypeId,
                            ContainerTypeName = e.ContainerTypeName,
                            MillRunSheetId = e.MillRunSheetId,
                        }).ToListAsync(ct)).ToList();
                }

                // Change the database Ticket model into the ViewModel to and return it
                return foundTickets
                    .Select(e => new TrackingTicketViewModel()
                    {
                        Id = e.Id,
                        QrCodeGuid = e.QrCodeGuid,
                        TechName = e.TechName,
                        ProductName = e.ProductName,
                        FormulationDescriptionType = e.FormulationDescriptionType,
                        ItemCategories =
                            (itemCats?.Count ?? 0) == 0
                            ? new List<ItemCategory>()
                            {
                                new ItemCategory()
                                {
                                    Id = e.ItemCategoryId,
                                    Name = e.ItemCategoryName
                                }
                            }
                            : itemCats,
                        Item =
                            ((items?.Count ?? 0) == 0
                            ? null
                            : items.SingleOrDefault(i => i.Id == e.ItemId))
                            ?? new TracklessItemViewModel()
                            {
                                Id = e.ItemId,
                                CategoryId = e.ItemCategoryId,
                                CategoryName = e.ItemCategoryName
                            }
                    }).ToList();
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        [HttpGet("{qrCodeGuid}")]
        public async Task<TrackingTicketViewModel> GetTrackingTicket(string qrCodeGuid, CancellationToken ct)
        {
            try
            {
                // Initialize the lookup list of Item Categories
                var itemCats = await context.ItemCategories.ToListAsync();

                // If no QrCodeGuid was specified, return a new ViewModel 
                // with the list of Item Categories populated
                if (!Guid.TryParse(qrCodeGuid, out Guid qrCode))
                {
                    return new TrackingTicketViewModel()
                    {
                        ItemCategories = itemCats
                    };
                }

                TrackingTicketViewModel retTicket = null;
                TracklessItemViewModel item = null;
                // Look for an existing Ticket with the provided QrCodeGuid
                var foundTicket = await context.TrackingTickets
                    .SingleOrDefaultAsync(e => e.QrCodeGuid == qrCode);
                if (foundTicket is { })
                {
                    // Initialize the return View Model
                    retTicket = new TrackingTicketViewModel()
                    {
                        QrCodeGuid = foundTicket.QrCodeGuid,
                        PlantId = foundTicket.PlantId,
                        TechName = foundTicket.TechName,
                        ProductName = foundTicket.ProductName,
                        FormulationDescriptionType = foundTicket.FormulationDescriptionType,
                        ItemCategories = itemCats
                    };

                    // If there was no CategoryName stored with the Ticket Id, we don't know which type of
                    // item the Ticket is for, so we will create a new Ticket using the model's Item information
                    if (string.IsNullOrWhiteSpace(foundTicket.ItemCategoryName) || string.IsNullOrWhiteSpace(foundTicket.ItemId))
                    {
                        item = new TracklessItemViewModel();
                    }
                    else
                    {
                        // A Category Name was stored with the Ticket record, so load the matching Item record
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
                                    CategoryId = e.CategoryId,
                                    CategoryName = e.CategoryName,
                                    Name = e.Name,
                                    Instructions = e.Instructions,
                                    Comments = e.Comments
                                }).SingleOrDefaultAsync(e => e.Id == foundTicket.ItemId),

                            "Sample" =>
                                await context.Samples.Select(e => new TracklessItemViewModel()
                                {
                                    Id = e.Id,
                                    CategoryId = e.CategoryId,
                                    CategoryName = e.CategoryName,
                                    SampleRetreivedBy = e.SampleRetreivedBy,
                                    SampleDate = e.SampleDate,
                                    ContainerTypeId = e.ContainerTypeId,
                                    ContainerTypeName = e.ContainerTypeName,
                                    MillRunSheetId = e.MillRunSheetId
                                }).SingleOrDefaultAsync(e => e.Id == foundTicket.ItemId),
                            _ => new TracklessItemViewModel()
                        };
                    }

                    // Set the Item in the ViewModel to the found Item, or a new one if an item was not found
                    retTicket.Item = item ?? new TracklessItemViewModel();

                    // Populate the Plant information into the View model
                    if (!string.IsNullOrWhiteSpace(retTicket.PlantId))
                    {
                        retTicket.Plant = await context.Plants
                            .Where(e => e.Id == retTicket.PlantId)
                            .Select(e => new PlantViewModel()
                            {
                                PlantId = e.Id,
                                PlantName = e.Name,
                                PlantAcronym = e.Acronym
                            }).SingleOrDefaultAsync();
                    }
                }

                // Return the View Model created by this process
                return retTicket
                    ?? new TrackingTicketViewModel()
                    {
                        QrCodeGuid = qrCode,
                        ItemCategories = itemCats,
                        NewTicket = true
                    };
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateTrackingTicket(TrackingTicketViewModel model)
        {
            var resp = new SaveTicketResponseModel();
            // TODO: Research how to handle transactions in Cosmos DB from the Client better
            // Maybe a stored procedure?
            // https://github.com/dotnet/efcore/issues/16836
            //IDbContextTransaction trans = null;
            try
            {
                //trans = await context.Database.BeginTransactionAsync();
                // Get all of the related records from the database for the passed in information
                var curTicket = await context.TrackingTickets.SingleOrDefaultAsync(e => e.QrCodeGuid == model.QrCodeGuid);
                // Keep track of whether a ticket record was found or not
                var newTicket = curTicket is null;

                // Try to find the corresponding Item record based on the Model's Item Id passed
                //   NOTE: this will not allow for changing an Item's Category Type.  The application should
                //         create a new Item Id if the user is trying to change the Item's Type, say from a 
                //         Sample to a Chemical
                ITrackingItem itemFromModel = string.IsNullOrWhiteSpace(model.ItemId)
                    ? (ITrackingItem)null
                    : model.ItemCategoryName switch
                    {
                        "Chemical" =>
                            await context.Chemicals.SingleOrDefaultAsync(e => e.Id == model.ItemId),
                        "Instrument" =>
                            await context.Instruments.SingleOrDefaultAsync(e => e.Id == model.ItemId),
                        "Sample" =>
                            await context.Samples.SingleOrDefaultAsync(e => e.Id == model.ItemId),
                        _ => null // TODO: Possibly throw an error for missing Category
                    };
                // Track whether this will be a new item or not
                var newItem = itemFromModel is null;

                // Try to find the Item Type already associated with the record from the database
                ITrackingItem itemFromDbTicket = string.IsNullOrWhiteSpace(curTicket?.ItemId)
                    ? (ITrackingItem)null
                    : curTicket.ItemCategoryName switch
                    {
                        "Chemical" =>
                            await context.Chemicals.SingleOrDefaultAsync(e => e.Id == curTicket.ItemId),
                        "Instrument" =>
                            await context.Instruments.SingleOrDefaultAsync(e => e.Id == curTicket.ItemId),
                        "Sample" =>
                            await context.Samples.SingleOrDefaultAsync(e => e.Id == curTicket.ItemId),
                        _ => null // TODO: Possibly throw an error for missing Category
                    };

                // If an Item was not found using the ItemId provided with the model (which could be blank)
                // but there is an Item ViewModel provided with the passed in model, then create a new Item
                // record.
                if (itemFromModel is null && model.Item is { } && !string.IsNullOrWhiteSpace(model.ItemCategoryName))
                {
                    // This will be a new Item to be referenced by the ticket
                    switch (model.ItemCategoryName)
                    {
                        case "Chemical":
                            itemFromModel = new Chemical()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = model.Item.Name,
                                CommonName = model.Item.CommonName
                            };
                            break;
                        case "Instrument":
                            itemFromModel = new Instrument()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = model.Item.Name,
                                Comments = model.Item.Comments,
                                Instructions = model.Item.Instructions
                            };
                            break;
                        case "Sample":
                            itemFromModel = new Sample()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MillRunSheetId = model.Item.MillRunSheetId,
                                SampleDate = model.Item.SampleDate,
                                SampleRetreivedBy = model.Item.SampleRetreivedBy,
                                ContainerTypeId = model.Item.ContainerTypeId,
                                ContainerTypeName = model.Item.ContainerTypeName
                            };
                            break;
                        default: break; // TODO: Possibly throw an error for missing Category
                    };
                }

                if (newTicket)
                {
                    // Initialize the new ticket object for the database
                    curTicket = new TrackingTicket()
                    {
                        Id = Guid.NewGuid().ToString(),
                        QrCodeGuid = model.QrCodeGuid
                    };
                }
                if (itemFromModel is { })
                {
                    // If there is an item provided by the model, whether new or existing, then
                    // update the values in the ticket that will be saved to the database
                    itemFromModel.TrackingTicketId = curTicket.Id;
                    itemFromModel.CategoryId = model.Item.CategoryId;
                    itemFromModel.CategoryName = model.Item.CategoryName;
                }

                // Update the values of the Ticket using the top level information from the model
                curTicket.TechName = model.TechName;
                curTicket.ProductName = model.ProductName;
                curTicket.FormulationDescriptionType = model.FormulationDescriptionType;

                // Update the ticket's top-level Item information using the new or null Item
                curTicket.ItemId = itemFromModel?.Id;
                curTicket.ItemCategoryId = itemFromModel?.CategoryId;
                curTicket.ItemCategoryName = itemFromModel?.CategoryName;

                // If there was an item already associated with the ticket found in the database,
                // and its Id does not match the ItemId passed with the model
                if (itemFromDbTicket is { } && curTicket.ItemId != itemFromDbTicket?.Id)
                {
                    // The referenced Item on the Ticket is being changed to a different Item
                    //   so remove the reference to the tracking ticket from the existing Item 
                    //   in the database
                    itemFromDbTicket.TrackingTicketId = null;
                    context.Entry(itemFromDbTicket).State = EntityState.Modified;
                }

                // Create or Update the referenced Document Records
                #region Plant info
                if (!string.IsNullOrWhiteSpace(model.PlantId))
                {
                    // A plant was associated with this ticket, so add the plant information
                    if (model.Plant is null || model.Plant.PlantId != model.PlantId)
                    {
                        // The plant information in the ViewModel does not match the plant Id specified
                        //   at the top level of the Ticket, or the Plant View Model was not populated,
                        //   so get the information from the Database for the plant
                        model.Plant = await context.Plants
                            .Where(e => e.Id == model.PlantId)
                            .Select(e => new PlantViewModel()
                            {
                                PlantId = e.Id,
                                PlantName = e.Name,
                                PlantAcronym = e.Acronym
                            }).SingleOrDefaultAsync();

                        // TODO: If Plant is still null, then that means a plant does not exist in the 
                        //       database with the supplied Id.  We should not try to create a new plant
                        //       here.  That should be handled by another Maintenance type page
                    }
                }

                curTicket.PlantId = model.PlantId;
                curTicket.PlantName = model.Plant?.PlantName;
                curTicket.PlantAcronym = model.Plant?.PlantAcronym;
                #endregion

                if (newTicket)
                {
                    // Add the new ticket if it was a new ticket
                    context.TrackingTickets.Add(curTicket);
                }
                else
                {
                    // Mark the exiting ticket record as updated
                    context.Entry(curTicket).State = EntityState.Modified;
                }
                if (newItem)
                {
                    // Add the new item if it was a newly created Item
                    switch (itemFromModel.CategoryName)
                    {
                        case "Chemical":
                            context.Chemicals.Add(itemFromModel as Chemical);
                            break;
                        case "Instrument":
                            context.Instruments.Add(itemFromModel as Instrument);
                            break;
                        case "Sample":
                            context.Samples.Add(itemFromModel as Sample);
                            break;
                    }
                }
                else
                {
                    // Mark the exiting item as updated
                    context.Entry(itemFromModel).State = EntityState.Modified;
                }
                // Save the changes to the database
                await context.SaveChangesAsync();
                //await trans.CommitAsync();

                // Repopulate the viewmodel to return with this process
                resp.ReturnItem = await GetTrackingTicket(curTicket.QrCodeGuid.ToString(), default);
            }
            catch (Exception ex)
            {
                //if (trans is { })
                //{
                //    await trans.RollbackAsync();
                //}
                resp.WasError = true;
                resp.ErrorMessages = new List<string>()
                {
                    "An error occurred while attempting to save the ticket."
                };
#if DEBUG
                resp.ErrorMessages.Add($"{ex.Message}:\r\n{ex.StackTrace}");
                while (ex.InnerException is { })
                {
                    ex = ex.InnerException;
                    resp.ErrorMessages.Add($"Inner Exception: {ex.Message}:\r\n{ex.StackTrace}");
                }
#endif
            }
            finally
            {
                //if (trans is { })
                //{
                //    await trans.DisposeAsync();
                //    trans = null;
                //}
            }

            return Ok(resp);
        }
    }
}
