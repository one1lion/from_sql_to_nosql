using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TracklessProductTracker.Server.Migrations
{
    public partial class InitialCrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chemical",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    TrackingTicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chemical", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContainerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 80, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    TrackingTicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Acronym = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TankId = table.Column<string>(maxLength: 60, nullable: true),
                    CurrentGallonsInTank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MillRunSheet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MillRunSheetId = table.Column<string>(maxLength: 60, nullable: true),
                    TankId = table.Column<int>(nullable: false),
                    BatchId = table.Column<string>(maxLength: 60, nullable: true),
                    Lot = table.Column<string>(maxLength: 60, nullable: true),
                    DateTimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MillRunSheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MillRunSheet_Tank_TankId",
                        column: x => x.TankId,
                        principalTable: "Tank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleRetreivedBy = table.Column<string>(maxLength: 80, nullable: true),
                    SampleDate = table.Column<DateTime>(nullable: false),
                    ContainerTypeId = table.Column<int>(nullable: true),
                    TrackingTicketId = table.Column<int>(nullable: true),
                    MillRunSheetId = table.Column<int>(nullable: false),
                    DateTimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sample_ContainerType_ContainerTypeId",
                        column: x => x.ContainerTypeId,
                        principalTable: "ContainerType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sample_MillRunSheet_MillRunSheetId",
                        column: x => x.MillRunSheetId,
                        principalTable: "MillRunSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SampleTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleId = table.Column<int>(nullable: false),
                    TestId = table.Column<int>(nullable: false),
                    GallonsInTank = table.Column<long>(nullable: true),
                    AsphaltSupplier = table.Column<string>(maxLength: 255, nullable: true),
                    TestSampleType = table.Column<string>(maxLength: 60, nullable: true),
                    TestNumber = table.Column<string>(maxLength: 60, nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SampleTest_Sample_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Sample",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SampleTest_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackingTicket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QrCodeGuid = table.Column<Guid>(nullable: false),
                    ItemCategoryId = table.Column<int>(nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    TechName = table.Column<string>(maxLength: 80, nullable: true),
                    ProductName = table.Column<string>(maxLength: 255, nullable: true),
                    FormulationDescriptionType = table.Column<string>(nullable: true),
                    ChemicalId = table.Column<int>(nullable: true),
                    InstrumentId = table.Column<int>(nullable: true),
                    SampleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingTicket_Chemical_ChemicalId",
                        column: x => x.ChemicalId,
                        principalTable: "Chemical",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackingTicket_Instrument_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackingTicket_ItemCategory_ItemCategoryId",
                        column: x => x.ItemCategoryId,
                        principalTable: "ItemCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackingTicket_Plant_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackingTicket_Sample_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Sample",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SampleTestMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleTestId = table.Column<int>(nullable: false),
                    TestMethodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleTestMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SampleTestMethod_SampleTest_SampleTestId",
                        column: x => x.SampleTestId,
                        principalTable: "SampleTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SampleTestMethod_TestMethod_TestMethodId",
                        column: x => x.TestMethodId,
                        principalTable: "TestMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContainerType",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "A 1 Liter Flask", "1L Flask" },
                    { 2, "A 250 milileter Beaker", "250mL Beaker" },
                    { 3, "A typical test tube", "Test Tube" }
                });

            migrationBuilder.InsertData(
                table: "ItemCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chemical" },
                    { 2, "Instrument" },
                    { 3, "Sample" }
                });

            migrationBuilder.InsertData(
                table: "Plant",
                columns: new[] { "Id", "Acronym", "Name" },
                values: new object[,]
                {
                    { 1, "PL1", "Plant 1" },
                    { 2, "PL2", "Plant 2" },
                    { 3, "PL3", "Plant 3" }
                });

            migrationBuilder.InsertData(
                table: "Tank",
                columns: new[] { "Id", "CurrentGallonsInTank", "TankId" },
                values: new object[] { 1, 2000, "Some-Internal-Id" });

            migrationBuilder.InsertData(
                table: "Test",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Test 1" },
                    { 2, "Test 2" },
                    { 3, "Test 3" }
                });

            migrationBuilder.InsertData(
                table: "TestMethod",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Test Method 1" },
                    { 2, "Test Method 2" },
                    { 3, "Test Method 3" }
                });

            migrationBuilder.InsertData(
                table: "MillRunSheet",
                columns: new[] { "Id", "BatchId", "DateTimeStamp", "Lot", "MillRunSheetId", "TankId" },
                values: new object[] { 1, "Internal-Batch-Id", new DateTime(2020, 9, 18, 22, 29, 30, 721, DateTimeKind.Utc).AddTicks(2745), "The-Lot-Num", "Some-Concat-Id", 1 });

            migrationBuilder.InsertData(
                table: "Sample",
                columns: new[] { "Id", "ContainerTypeId", "DateTimeStamp", "MillRunSheetId", "SampleDate", "SampleRetreivedBy", "TrackingTicketId" },
                values: new object[] { 1, 2, new DateTime(2020, 9, 18, 22, 29, 30, 721, DateTimeKind.Utc).AddTicks(8650), 1, new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Humberto", 1 });

            migrationBuilder.InsertData(
                table: "TrackingTicket",
                columns: new[] { "Id", "ChemicalId", "FormulationDescriptionType", "InstrumentId", "ItemCategoryId", "PlantId", "ProductName", "QrCodeGuid", "SampleId", "TechName" },
                values: new object[] { 1, null, null, null, 3, 1, "Some product name, free-form entered", new Guid("a45f2eda-4688-417e-acd2-8eb01bc2702a"), 1, "Beruto" });

            migrationBuilder.CreateIndex(
                name: "IX_MillRunSheet_MillRunSheetId",
                table: "MillRunSheet",
                column: "MillRunSheetId",
                unique: true,
                filter: "[MillRunSheetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MillRunSheet_TankId",
                table: "MillRunSheet",
                column: "TankId");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_ContainerTypeId",
                table: "Sample",
                column: "ContainerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_MillRunSheetId",
                table: "Sample",
                column: "MillRunSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleTest_SampleId",
                table: "SampleTest",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleTest_TestId",
                table: "SampleTest",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleTestMethod_SampleTestId",
                table: "SampleTestMethod",
                column: "SampleTestId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleTestMethod_TestMethodId",
                table: "SampleTestMethod",
                column: "TestMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTicket_ChemicalId",
                table: "TrackingTicket",
                column: "ChemicalId",
                unique: true,
                filter: "[ChemicalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTicket_InstrumentId",
                table: "TrackingTicket",
                column: "InstrumentId",
                unique: true,
                filter: "[InstrumentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTicket_ItemCategoryId",
                table: "TrackingTicket",
                column: "ItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTicket_PlantId",
                table: "TrackingTicket",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTicket_SampleId",
                table: "TrackingTicket",
                column: "SampleId",
                unique: true,
                filter: "[SampleId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SampleTestMethod");

            migrationBuilder.DropTable(
                name: "TrackingTicket");

            migrationBuilder.DropTable(
                name: "SampleTest");

            migrationBuilder.DropTable(
                name: "TestMethod");

            migrationBuilder.DropTable(
                name: "Chemical");

            migrationBuilder.DropTable(
                name: "Instrument");

            migrationBuilder.DropTable(
                name: "ItemCategory");

            migrationBuilder.DropTable(
                name: "Plant");

            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "ContainerType");

            migrationBuilder.DropTable(
                name: "MillRunSheet");

            migrationBuilder.DropTable(
                name: "Tank");
        }
    }
}
