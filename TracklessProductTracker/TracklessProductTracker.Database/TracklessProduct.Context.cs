using Microsoft.EntityFrameworkCore;
using System;
using TracklessProductTracker.Models;

namespace TracklessProductTracker.Database
{
    public class TracklessProductContext : DbContext
    {
        #region Constructors
        public TracklessProductContext() { }

        public TracklessProductContext(DbContextOptions<TracklessProductContext> options) : base(options) { }
        #endregion

        #region Models
        public DbSet<Chemical> Chemicals { get; set; }
        public DbSet<ContainerType> ContainerTypes { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<MillRunSheet> MillRunSheets { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<SampleTest> SampleTests { get; set; }
        public DbSet<SampleTestMethod> SampleTestMethods { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestMethod> TestMethods { get; set; }
        public DbSet<TrackingTicket> TrackingTickets { get; set; }
        #endregion

        #region Overridden Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DBContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chemical>(entity =>
            {
                entity.ToTable("Chemical");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(255);

                entity.HasOne(e => e.TrackingTicket)
                    .WithOne(e => e.Chemical)
                    .HasForeignKey<Chemical>("TrackingTicketId")
                    .IsRequired(false);
            });

            modelBuilder.Entity<ContainerType>(entity =>
            {
                entity.ToTable("ContainerType");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(255);

                entity.HasMany(e => e.Samples)
                    .WithOne(e => e.ContainerType)
                    .HasForeignKey(e => e.ContainerTypeId);
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.ToTable("Instrument");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(255);

                entity.HasOne(e => e.TrackingTicket)
                    .WithOne(e => e.Instrument)
                    .HasForeignKey<Instrument>("TrackingTicketId")
                    .IsRequired(false);
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.ToTable("ItemCategory");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(255);

                entity.HasMany(e => e.TrackingTickets)
                    .WithOne(e => e.ItemCategory)
                    .HasForeignKey(e => e.ItemCategoryId);
            });

            modelBuilder.Entity<MillRunSheet>(entity =>
            {
                entity.ToTable("MillRunSheet");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.MillRunSheetId)
                    .HasMaxLength(60);

                entity.HasIndex(e => e.MillRunSheetId)
                    .IsUnique();

                entity.Property(e => e.BatchId)
                    .HasMaxLength(60);

                entity.Property(e => e.Lot)
                    .HasMaxLength(60);

                entity.HasOne(e => e.Tank)
                    .WithMany(e => e.MillRunSheets)
                    .HasForeignKey(e => e.TankId);

                entity.HasMany(e => e.Samples)
                    .WithOne(e => e.MillRunSheet)
                    .HasForeignKey(e => e.MillRunSheetId);
            });

            modelBuilder.Entity<Plant>(entity =>
            {
                entity.ToTable("Plant");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Acronym)
                    .HasMaxLength(20);

                entity.HasMany(e => e.TrackingTickets)
                    .WithOne(e => e.Plant)
                    .HasForeignKey(e => e.PlantId);
            });

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.ToTable("Sample");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.SampleRetreivedBy)
                    .HasMaxLength(80);

                entity.HasOne(e => e.ContainerType)
                    .WithMany(e => e.Samples)
                    .HasForeignKey(e => e.ContainerTypeId);

                entity.HasOne(e => e.MillRunSheet)
                    .WithMany(e => e.Samples)
                    .HasForeignKey(e => e.MillRunSheetId);

                entity.HasOne(e => e.TrackingTicket)
                    .WithOne(e => e.Sample)
                    .HasForeignKey<Sample>("TrackingTicketId")
                    .IsRequired(false);
            });

            modelBuilder.Entity<SampleTest>(entity =>
            {
                entity.ToTable("SampleTest");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.AsphaltSupplier)
                    .HasMaxLength(255);

                entity.Property(e => e.TestNumber)
                    .HasMaxLength(60);

                entity.Property(e => e.TestSampleType)
                    .HasMaxLength(60);

                entity.HasOne(e => e.Sample)
                    .WithMany(e => e.SampleTests)
                    .HasForeignKey(e => e.SampleId);

                entity.HasOne(e => e.Test)
                    .WithMany(e => e.SampleTests)
                    .HasForeignKey(e => e.TestId);

                entity.HasMany(e => e.SampleTestMethods)
                    .WithOne(e => e.SampleTest)
                    .HasForeignKey(e => e.SampleTestId);
            });

            modelBuilder.Entity<SampleTestMethod>(entity =>
            {
                entity.ToTable("SampleTestMethod");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.HasOne(e => e.SampleTest)
                    .WithMany(e => e.SampleTestMethods)
                    .HasForeignKey(e => e.SampleTestId);

                entity.HasOne(e => e.TestMethod)
                    .WithMany(e => e.SampleTestMethods)
                    .HasForeignKey(e => e.TestMethodId);
            });

            modelBuilder.Entity<Tank>(entity =>
            {
                entity.ToTable("Tank");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.TankId)
                    .HasMaxLength(60);

                entity.HasMany(e => e.MillRunSheets)
                    .WithOne(e => e.Tank)
                    .HasForeignKey(e => e.TankId);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("Test");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.HasMany(e => e.SampleTests)
                    .WithOne(e => e.Test)
                    .HasForeignKey(e => e.TestId);
            });

            modelBuilder.Entity<TestMethod>(entity =>
            {
                entity.ToTable("TestMethod");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.HasMany(e => e.SampleTestMethods)
                    .WithOne(e => e.TestMethod)
                    .HasForeignKey(e => e.TestMethodId);
            });

            modelBuilder.Entity<TrackingTicket>(entity =>
            {
                entity.ToTable("TrackingTicket");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.QrCodeGuid)
                    .IsRequired();

                entity.Property(e => e.TechName)
                    .HasMaxLength(80);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255);

                entity.HasOne(e => e.Chemical)
                    .WithOne(e => e.TrackingTicket)
                    .HasForeignKey<TrackingTicket>("ChemicalId")
                    .IsRequired(false);

                entity.HasOne(e => e.Instrument)
                    .WithOne(e => e.TrackingTicket)
                    .HasForeignKey<TrackingTicket>("InstrumentId")
                    .IsRequired(false);

                entity.HasOne(e => e.ItemCategory)
                    .WithMany(e => e.TrackingTickets)
                    .HasForeignKey(e => e.ItemCategoryId);

                entity.HasOne(e => e.Plant)
                    .WithMany(e => e.TrackingTickets)
                    .HasForeignKey(e => e.PlantId);

                entity.HasOne(e => e.Sample)
                    .WithOne(e => e.TrackingTicket)
                    .HasForeignKey<TrackingTicket>("SampleId")
                    .IsRequired(false);
            });

            ConfigureSeedData(modelBuilder);
        }

        private void ConfigureSeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContainerType>().HasData(
                new ContainerType() { Id = 1, Name = "1L Flask", Description = "A 1 Liter Flask" },
                new ContainerType() { Id = 2, Name = "250mL Beaker", Description = "A 250 milileter Beaker" },
                new ContainerType() { Id = 3, Name = "Test Tube", Description = "A typical test tube" }
            );

            modelBuilder.Entity<ItemCategory>().HasData(
                new ItemCategory() { Id = 1, Name = "Chemical" },
                new ItemCategory() { Id = 2, Name = "Instrument" },
                new ItemCategory() { Id = 3, Name = "Sample" }
            );

            modelBuilder.Entity<Plant>().HasData(
                new Plant() { Id = 1, Name = "Plant 1", Acronym = "PL1" },
                new Plant() { Id = 2, Name = "Plant 2", Acronym = "PL2" },
                new Plant() { Id = 3, Name = "Plant 3", Acronym = "PL3" }
            );

            modelBuilder.Entity<Test>().HasData(
                new Test() { Id = 1, Name = "Test 1" },
                new Test() { Id = 2, Name = "Test 2" },
                new Test() { Id = 3, Name = "Test 3" }
            );

            modelBuilder.Entity<TestMethod>().HasData(
                new TestMethod() { Id = 1, Name = "Test Method 1" },
                new TestMethod() { Id = 2, Name = "Test Method 2" },
                new TestMethod() { Id = 3, Name = "Test Method 3" }
            );

            modelBuilder.Entity<Tank>().HasData(
                new Tank()
                {
                    Id = 1,
                    TankId = "Some-Internal-Id",
                    CurrentGallonsInTank = 2000
                }
            );

            modelBuilder.Entity<MillRunSheet>().HasData(
                new MillRunSheet()
                {
                    Id = 1,
                    MillRunSheetId = "Some-Concat-Id",
                    TankId = 1,
                    DateTimeStamp = DateTime.UtcNow,
                    BatchId = "Internal-Batch-Id",
                    Lot = "The-Lot-Num"
                }
            );

            modelBuilder.Entity<TrackingTicket>().HasData(
                new TrackingTicket()
                {
                    Id = 1,
                    QrCodeGuid = Guid.Parse("A45F2EDA-4688-417E-ACD2-8EB01BC2702A"),
                    ItemCategoryId = 3,
                    SampleId = 1,
                    PlantId = 1,
                    ProductName = "Some product name, free-form entered",
                    TechName = "Beruto"
                }
            );

            modelBuilder.Entity<Sample>().HasData(
                new Sample()
                {
                    Id = 1,
                    ContainerTypeId = 2,
                    DateTimeStamp = DateTime.UtcNow,
                    MillRunSheetId = 1,
                    SampleDate = new DateTime(2020, 2, 1),
                    SampleRetreivedBy = "Humberto",
                    TrackingTicketId = 1
                }
            );
        }
        #endregion
    }
}
