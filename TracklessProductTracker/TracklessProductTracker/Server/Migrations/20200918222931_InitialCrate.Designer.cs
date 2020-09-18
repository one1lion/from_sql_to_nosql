﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TracklessProductTracker.Database;

namespace TracklessProductTracker.Server.Migrations
{
    [DbContext(typeof(TracklessProductContext))]
    [Migration("20200918222931_InitialCrate")]
    partial class InitialCrate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TracklessProductTracker.Models.Chemical", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("TrackingTicketId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Chemical");
                });

            modelBuilder.Entity("TracklessProductTracker.Models.ContainerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("ContainerType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "A 1 Liter Flask",
                            Name = "1L Flask"
                        },
                        new
                        {
                            Id = 2,
                            Description = "A 250 milileter Beaker",
                            Name = "250mL Beaker"
                        },
                        new
                        {
                            Id = 3,
                            Description = "A typical test tube",
                            Name = "Test Tube"
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.Instrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("TrackingTicketId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Instrument");
                });

            modelBuilder.Entity("TracklessProductTracker.Models.ItemCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("ItemCategory");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Chemical"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Instrument"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Sample"
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.MillRunSheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BatchId")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<DateTime>("DateTimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lot")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("MillRunSheetId")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<int>("TankId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MillRunSheetId")
                        .IsUnique()
                        .HasFilter("[MillRunSheetId] IS NOT NULL");

                    b.HasIndex("TankId");

                    b.ToTable("MillRunSheet");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BatchId = "Internal-Batch-Id",
                            DateTimeStamp = new DateTime(2020, 9, 18, 22, 29, 30, 721, DateTimeKind.Utc).AddTicks(2745),
                            Lot = "The-Lot-Num",
                            MillRunSheetId = "Some-Concat-Id",
                            TankId = 1
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.Plant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Acronym")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Plant");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Acronym = "PL1",
                            Name = "Plant 1"
                        },
                        new
                        {
                            Id = 2,
                            Acronym = "PL2",
                            Name = "Plant 2"
                        },
                        new
                        {
                            Id = 3,
                            Acronym = "PL3",
                            Name = "Plant 3"
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.Sample", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContainerTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("MillRunSheetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SampleDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SampleRetreivedBy")
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<int?>("TrackingTicketId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContainerTypeId");

                    b.HasIndex("MillRunSheetId");

                    b.ToTable("Sample");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContainerTypeId = 2,
                            DateTimeStamp = new DateTime(2020, 9, 18, 22, 29, 30, 721, DateTimeKind.Utc).AddTicks(8650),
                            MillRunSheetId = 1,
                            SampleDate = new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SampleRetreivedBy = "Humberto",
                            TrackingTicketId = 1
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.SampleTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AsphaltSupplier")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("GallonsInTank")
                        .HasColumnType("bigint");

                    b.Property<int>("SampleId")
                        .HasColumnType("int");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("TestNumber")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("TestSampleType")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.HasIndex("SampleId");

                    b.HasIndex("TestId");

                    b.ToTable("SampleTest");
                });

            modelBuilder.Entity("TracklessProductTracker.Models.SampleTestMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SampleTestId")
                        .HasColumnType("int");

                    b.Property<int>("TestMethodId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SampleTestId");

                    b.HasIndex("TestMethodId");

                    b.ToTable("SampleTestMethod");
                });

            modelBuilder.Entity("TracklessProductTracker.Models.Tank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CurrentGallonsInTank")
                        .HasColumnType("int");

                    b.Property<string>("TankId")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.ToTable("Tank");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CurrentGallonsInTank = 2000,
                            TankId = "Some-Internal-Id"
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Test");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Test 1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Test 2"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Test 3"
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.TestMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("TestMethod");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Test Method 1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Test Method 2"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Test Method 3"
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.TrackingTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ChemicalId")
                        .HasColumnType("int");

                    b.Property<string>("FormulationDescriptionType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InstrumentId")
                        .HasColumnType("int");

                    b.Property<int>("ItemCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("PlantId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<Guid>("QrCodeGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("SampleId")
                        .HasColumnType("int");

                    b.Property<string>("TechName")
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.HasIndex("ChemicalId")
                        .IsUnique()
                        .HasFilter("[ChemicalId] IS NOT NULL");

                    b.HasIndex("InstrumentId")
                        .IsUnique()
                        .HasFilter("[InstrumentId] IS NOT NULL");

                    b.HasIndex("ItemCategoryId");

                    b.HasIndex("PlantId");

                    b.HasIndex("SampleId")
                        .IsUnique()
                        .HasFilter("[SampleId] IS NOT NULL");

                    b.ToTable("TrackingTicket");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ItemCategoryId = 3,
                            PlantId = 1,
                            ProductName = "Some product name, free-form entered",
                            QrCodeGuid = new Guid("a45f2eda-4688-417e-acd2-8eb01bc2702a"),
                            SampleId = 1,
                            TechName = "Beruto"
                        });
                });

            modelBuilder.Entity("TracklessProductTracker.Models.MillRunSheet", b =>
                {
                    b.HasOne("TracklessProductTracker.Models.Tank", "Tank")
                        .WithMany("MillRunSheets")
                        .HasForeignKey("TankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TracklessProductTracker.Models.Sample", b =>
                {
                    b.HasOne("TracklessProductTracker.Models.ContainerType", "ContainerType")
                        .WithMany("Samples")
                        .HasForeignKey("ContainerTypeId");

                    b.HasOne("TracklessProductTracker.Models.MillRunSheet", "MillRunSheet")
                        .WithMany("Samples")
                        .HasForeignKey("MillRunSheetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TracklessProductTracker.Models.SampleTest", b =>
                {
                    b.HasOne("TracklessProductTracker.Models.Sample", "Sample")
                        .WithMany("SampleTests")
                        .HasForeignKey("SampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TracklessProductTracker.Models.Test", "Test")
                        .WithMany("SampleTests")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TracklessProductTracker.Models.SampleTestMethod", b =>
                {
                    b.HasOne("TracklessProductTracker.Models.SampleTest", "SampleTest")
                        .WithMany("SampleTestMethods")
                        .HasForeignKey("SampleTestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TracklessProductTracker.Models.TestMethod", "TestMethod")
                        .WithMany("SampleTestMethods")
                        .HasForeignKey("TestMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TracklessProductTracker.Models.TrackingTicket", b =>
                {
                    b.HasOne("TracklessProductTracker.Models.Chemical", "Chemical")
                        .WithOne("TrackingTicket")
                        .HasForeignKey("TracklessProductTracker.Models.TrackingTicket", "ChemicalId");

                    b.HasOne("TracklessProductTracker.Models.Instrument", "Instrument")
                        .WithOne("TrackingTicket")
                        .HasForeignKey("TracklessProductTracker.Models.TrackingTicket", "InstrumentId");

                    b.HasOne("TracklessProductTracker.Models.ItemCategory", "ItemCategory")
                        .WithMany("TrackingTickets")
                        .HasForeignKey("ItemCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TracklessProductTracker.Models.Plant", "Plant")
                        .WithMany("TrackingTickets")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TracklessProductTracker.Models.Sample", "Sample")
                        .WithOne("TrackingTicket")
                        .HasForeignKey("TracklessProductTracker.Models.TrackingTicket", "SampleId");
                });
#pragma warning restore 612, 618
        }
    }
}
