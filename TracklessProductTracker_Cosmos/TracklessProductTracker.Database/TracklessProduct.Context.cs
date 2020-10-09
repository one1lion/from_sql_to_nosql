using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TracklessProductTracker.Models;

namespace TracklessProductTracker.Database
{
    public class TracklessProductContext : DbContext
    {
        public TracklessProductContext()
        {
        }
        public TracklessProductContext(DbContextOptions<TracklessProductContext> options) : base(options)
        {
        }

        #region Lookups
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        #endregion
        #region Plants
        public DbSet<Plant> Plants { get; set; }
        #endregion
        #region Tracking Items
        public DbSet<Chemical> Chemicals { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Sample> Samples { get; set; }
        #endregion
        #region Tracking Tickets 
        public DbSet<TrackingTicket> TrackingTickets { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use default Cosmos Emulator settings
                optionsBuilder.UseCosmos(
                    "https://localhost:8001",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    databaseName: "TracklessProduct");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Default Container
            modelBuilder.HasDefaultContainer("TrackingItems");
            #endregion

            #region Lookups
            modelBuilder.Entity<ItemCategory>(options =>
            {
                options.ToContainer("Lookups");

                options.HasDiscriminator(e => e.Type);
            });

            modelBuilder.Entity<Tank>(options =>
            {
                options.ToContainer("Lookups");

                options.HasDiscriminator(e => e.Type);
            });
            #endregion

            #region Mill Run Sheets
            modelBuilder.Entity<MillRunSheet>(options =>
            {
                options.ToContainer("MillRunSheets");

                options.HasPartitionKey(e => e.MillRunSheetId);
            });
            #endregion

            #region Plants
            modelBuilder.Entity<Plant>(options =>
            {
                options.ToContainer("Plants");

                options.OwnsMany(e =>
                    e.Addresses,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");
                    });

                options.OwnsMany(e =>
                    e.ContactPeople,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");

                        sa.OwnsMany(x =>
                            x.EmailAddresses,
                            sx =>
                            {
                                sx.ToJsonProperty("ContactInfo");
                            });

                        sa.OwnsMany(x =>
                            x.PhoneNumbers,
                            sx =>
                            {
                                sx.ToJsonProperty("ContactInfo");
                            });
                    });

                options.OwnsMany(e =>
                    e.EmailAddresses,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");
                    });

                options.OwnsMany(e =>
                    e.PhoneNumbers,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");
                    });
            });
            #endregion

            #region Tracking Items
            modelBuilder.Entity<Chemical>(options =>
            {
                options.ToContainer("TrackingItems");

                options.HasPartitionKey(e => e.Id);

                options.HasDiscriminator(e => e.Type);
            });

            modelBuilder.Entity<Instrument>(options =>
            {
                options.ToContainer("TrackingItems");

                options.HasPartitionKey(e => e.Id);

                options.HasDiscriminator(e => e.Type);
            });

            modelBuilder.Entity<Sample>(options =>
            {
                options.ToContainer("TrackingItems");

                options.HasPartitionKey(e => e.Id);

                options.HasDiscriminator(e => e.Type);

                options.OwnsMany(e =>
                    e.Tests,
                    sa =>
                    {
                        sa.ToJsonProperty("Tests");

                        sa.OwnsMany(x =>
                            x.Methods,
                            sx =>
                            {
                                sx.ToJsonProperty("Methods");
                            });
                    });
            });
            #endregion

            #region Tracking Tickets 
            modelBuilder.Entity<TrackingTicket>(options =>
            {
                options.ToContainer("TrackingTickets");

                options.HasPartitionKey(e => e.Id);
            });
            #endregion
        }
    }
}
