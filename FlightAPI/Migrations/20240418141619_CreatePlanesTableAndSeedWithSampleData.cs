using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlightAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreatePlanesTableAndSeedWithSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaneId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Planes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CruiseSpeed = table.Column<double>(type: "float", nullable: false),
                    Range = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlaneId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PlaneId", "PlaneType" },
                values: new object[] { 3, 0 });

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PlaneId", "PlaneType" },
                values: new object[] { 1, 0 });

            migrationBuilder.InsertData(
                table: "Planes",
                columns: new[] { "Id", "Capacity", "CruiseSpeed", "Manufacturer", "Model", "Range", "Type" },
                values: new object[,]
                {
                    { 1, 853, 903.0, "Airbus", "A380", 15700.0, "Airbus" },
                    { 2, 660, 920.0, "Boeing", "747", 13450.0, "Boeing" },
                    { 3, 132, 870.0, "Embraer", "E195", 3700.0, "Embraer" },
                    { 4, 104, 829.0, "Bombardier", "CRJ1000", 2752.0, "Bombardier" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_PlaneId",
                table: "Flights",
                column: "PlaneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Planes_PlaneId",
                table: "Flights",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Planes_PlaneId",
                table: "Flights");

            migrationBuilder.DropTable(
                name: "Planes");

            migrationBuilder.DropIndex(
                name: "IX_Flights_PlaneId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "PlaneId",
                table: "Flights");

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 2,
                column: "PlaneType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 3,
                column: "PlaneType",
                value: 2);
        }
    }
}
