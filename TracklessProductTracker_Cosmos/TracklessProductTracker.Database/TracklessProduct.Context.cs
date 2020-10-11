using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        public DbSet<ContainerType> ContainerTypes { get; set; }
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
            modelBuilder.Entity<ContainerType>(entity =>
            {
                entity.ToContainer("Lookups");

                entity.HasPartitionKey(e => e.Type);

                entity.HasKey(e => e.Id);

                entity.HasData(
                    new ContainerType() { Id = Guid.NewGuid().ToString(), Name = "1L Flask", Description = "A 1 Liter Flask" },
                    new ContainerType() { Id = "347BD99D-7C4B-44C2-B8EC-2A365B888BE9", Name = "250mL Beaker", Description = "A 250 milileter Beaker" },
                    new ContainerType() { Id = Guid.NewGuid().ToString(), Name = "Test Tube", Description = "A typical test tube" }
                );
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.ToContainer("Lookups");

                entity.HasPartitionKey(e => e.Type);

                entity.HasKey(e => e.Id);

                entity.HasData(
                    new ItemCategory() { Id = "60FD7286-FD07-40DC-A7BC-31DC6D706F89", Name = "Chemical" },
                    new ItemCategory() { Id = "447B9849-7B31-48BC-AEF3-7773B5847FE8", Name = "Instrument" },
                    new ItemCategory() { Id = "75B5D2C5-E0DA-410B-B6B5-FA0CDF5CFFC4", Name = "Sample" }
                );
            });

            modelBuilder.Entity<Tank>(entity =>
            {
                entity.ToContainer("Lookups");

                entity.HasPartitionKey(e => e.PlantId);
                entity.HasPartitionKey(e => e.Type);

                entity.HasKey(e => e.Id);

                entity.HasData(
                    new Tank()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TankId = "Some-Internal-Id",
                        CurrentGallonsInTank = 2000,
                        PlantId = "Plant1Id"
                    }
                );
            });
            #endregion

            #region Mill Run Sheets
            modelBuilder.Entity<MillRunSheet>(entity =>
            {
                entity.ToContainer("MillRunSheets");

                entity.HasPartitionKey(e => e.TankId);

                entity.HasNoDiscriminator();

                entity.HasKey(e => e.Id);

                entity.HasData(
                    new MillRunSheet()
                    {
                        Id = Guid.NewGuid().ToString(),
                        MillRunSheetId = "Some-Concat-Id",
                        TankId = "Some-Internal-Id",
                        DateTimeStamp = DateTime.UtcNow,
                        BatchId = "Internal-Batch-Id",
                        Lot = "The-Lot-Num"
                    }
                );
            });
            #endregion

            #region Plants
            modelBuilder.Entity<Plant>(entity =>
            {
                entity.ToContainer("Plants");

                entity.HasNoDiscriminator();

                entity.HasKey(e => e.Id);

                entity.HasData(
                    new Plant()
                    {
                        Id = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                        Name = "First Plant",
                        Acronym = "P1"
                    }
                );

                entity.OwnsMany(e =>
                    e.Addresses,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");

                        sa.HasKey(e => e.Id);

                        sa.HasData(
                            new Address()
                            {
                                Id = Guid.NewGuid().ToString(),
                                PlantId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                                AddressLine1 = "Line 1",
                                City = "Some City",
                                State_Province = "HE",
                                Zip = "12345"
                            }
                        );
                    });

                entity.OwnsMany(e =>
                    e.ContactPeople,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");

                        sa.HasKey(e => e.Id);

                        sa.HasData(
                            new Person()
                            {
                                Id = "02ABD9DA-4A39-448C-9687-610E497E4E74",
                                PlantId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                                Name = "Hanrick Jones"
                            }
                        );

                        sa.OwnsMany(x =>
                            x.EmailAddresses,
                            sx =>
                            {
                                sx.ToJsonProperty("ContactInfo");

                                sx.HasData(
                                    new EmailContact()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        PersonId = "02ABD9DA-4A39-448C-9687-610E497E4E74",
                                        PersonPlantId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                                        Email = "hanrick.jones@example.com",
                                        PreferredContactMethod = true
                                    }
                                );
                            });

                        sa.OwnsMany(x =>
                            x.PhoneNumbers,
                            sx =>
                            {
                                sx.ToJsonProperty("ContactInfo");

                                sx.HasKey(e => e.Id);
                            });
                    });

                entity.OwnsMany(e =>
                    e.EmailAddresses,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");

                        sa.HasKey(e => e.Id);
                    });

                entity.OwnsMany(e =>
                    e.PhoneNumbers,
                    sa =>
                    {
                        sa.ToJsonProperty("ContactInfo");

                        sa.HasKey(e => e.Id);
                    });
            });
            #endregion

            #region Tracking Items
            modelBuilder.Entity<Chemical>(entity =>
            {
                entity.ToContainer("TrackingItems");

                entity.HasPartitionKey(e => e.TrackingTicketId);

                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.ToContainer("TrackingItems");

                entity.HasPartitionKey(e => e.TrackingTicketId);

                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.ToContainer("TrackingItems");

                entity.HasPartitionKey(e => e.MillRunSheetId);
                entity.HasPartitionKey(e => e.TrackingTicketId);

                entity.HasKey(e => e.Id);

                entity.HasData(
                    new Sample()
                    {
                        Id = "A2D54EC0-3D62-4F33-AB84-21F25BA334DE",
                        ContainerTypeId = "347BD99D-7C4B-44C2-B8EC-2A365B888BE9",
                        ContainerTypeName = "250mL Beaker",
                        MillRunSheetId = "Some-Concat-Id",
                        SampleDate = new DateTime(2020, 2, 1),
                        SampleRetreivedBy = "Humberto",
                        TrackingTicketId = "5F4290DF-7873-4F50-8F03-39820D9788CB",
                        CategoryId = "75B5D2C5-E0DA-410B-B6B5-FA0CDF5CFFC4",
                        CategoryName = "Sample"
                    }
                );

                entity.OwnsMany(e =>
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
            modelBuilder.Entity<TrackingTicket>(entity =>
            {
                entity.ToContainer("TrackingTickets");

                entity.HasPartitionKey(e => e.ItemId);

                entity.HasNoDiscriminator();

                entity.HasKey(e => e.Id);

                entity.HasData(
                    new TrackingTicket()
                    {
                        Id = "5F4290DF-7873-4F50-8F03-39820D9788CB",
                        QrCodeGuid = Guid.Parse("A45F2EDA-4688-417E-ACD2-8EB01BC2702A"),
                        ItemCategoryId = "75B5D2C5-E0DA-410B-B6B5-FA0CDF5CFFC4",
                        ItemCategoryName = "Sample",
                        ItemId = "A2D54EC0-3D62-4F33-AB84-21F25BA334DE",
                        PlantId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                        PlantName = "First Plant",
                        PlantAcronym = "P1",
                        ProductName = "Some product name, free-form entered",
                        TechName = "Beruto"
                    }
                );
            });
            #endregion

            //SetupSeedData(modelBuilder);
        }

        void SetupSeedData(ModelBuilder modelBuilder)
        {
            #region Lookups
            #region Container Types
            modelBuilder.Entity<ContainerType>().HasData(
                new ContainerType() { Id = Guid.NewGuid().ToString(), Name = "1L Flask", Description = "A 1 Liter Flask" },
                new ContainerType() { Id = "347BD99D-7C4B-44C2-B8EC-2A365B888BE9", Name = "250mL Beaker", Description = "A 250 milileter Beaker" },
                new ContainerType() { Id = Guid.NewGuid().ToString(), Name = "Test Tube", Description = "A typical test tube" }
            );
            #endregion
            #region Item Categories
            modelBuilder.Entity<ItemCategory>().HasData(
                new ItemCategory() { Id = Guid.NewGuid().ToString(), Name = "Chemical" },
                new ItemCategory() { Id = Guid.NewGuid().ToString(), Name = "Instrument" },
                new ItemCategory() { Id = "75B5D2C5-E0DA-410B-B6B5-FA0CDF5CFFC4", Name = "Sample" }
            );
            #endregion
            #region Tanks
            modelBuilder.Entity<Tank>().HasData(
                new Tank()
                {
                    Id = Guid.NewGuid().ToString(),
                    TankId = "Some-Internal-Id",
                    CurrentGallonsInTank = 2000,
                    PlantId = "Plant1Id"
                }
            );
            #endregion
            #endregion

            #region Mill Run Sheets
            modelBuilder.Entity<MillRunSheet>().HasData(
                new MillRunSheet()
                {
                    Id = Guid.NewGuid().ToString(),
                    MillRunSheetId = "Some-Concat-Id",
                    TankId = "Some-Internal-Id",
                    DateTimeStamp = DateTime.UtcNow,
                    BatchId = "Internal-Batch-Id",
                    Lot = "The-Lot-Num"
                }
            );

            #endregion
            #region Plants
            modelBuilder.Entity<Plant>(entity =>
            {
                entity.HasData(
                    new Plant()
                    {
                        Id = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                        Name = "First Plant",
                        Acronym = "P1"
                    }
                );
            });
            modelBuilder.Entity<Address>().HasData(
                new Address()
                {
                    PlantId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                    AddressLine1 = "Line 1",
                    City = "Some City",
                    State_Province = "HE",
                    Zip = "12345"
                }
            );
            modelBuilder.Entity<Person>().HasData(
                new Person()
                {
                    PlantId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                    Name = "Hanrick Jones"
                }
            );
            modelBuilder.Entity<EmailContact>().HasData(
                new EmailContact()
                {
                    PersonId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                    Email = "hanrick.jones@example.com",
                    PreferredContactMethod = true
                }
            );
            #endregion
            #region Tracking Items
            modelBuilder.Entity<Sample>().HasData(
                new Sample()
                {
                    Id = "A2D54EC0-3D62-4F33-AB84-21F25BA334DE",
                    ContainerTypeId = "347BD99D-7C4B-44C2-B8EC-2A365B888BE9",
                    ContainerTypeName = "250mL Beaker",
                    MillRunSheetId = "Some-Concat-Id",
                    SampleDate = new DateTime(2020, 2, 1),
                    SampleRetreivedBy = "Humberto",
                    TrackingTicketId = "5F4290DF-7873-4F50-8F03-39820D9788CB"
                }
            );
            #endregion
            #region Tracking Tickets
            modelBuilder.Entity<TrackingTicket>().HasData(
                new TrackingTicket()
                {
                    Id = "5F4290DF-7873-4F50-8F03-39820D9788CB",
                    QrCodeGuid = Guid.Parse("A45F2EDA-4688-417E-ACD2-8EB01BC2702A"),
                    ItemCategoryId = "75B5D2C5-E0DA-410B-B6B5-FA0CDF5CFFC4",
                    ItemCategoryName = "Sample",
                    ItemId = "A2D54EC0-3D62-4F33-AB84-21F25BA334DE",
                    PlantId = "696FC4EC-AEE5-4F8E-A5DF-91E1573AE4D1",
                    PlantName = "First Plant",
                    PlantAcronym = "P1",
                    ProductName = "Some product name, free-form entered",
                    TechName = "Beruto"
                }
            );
            #endregion
        }
    }
}
