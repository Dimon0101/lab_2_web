using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lab_2_web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVisitorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Visitors_VisitorId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_VisitorId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "VisitorId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "VisitorName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VisitorPhone",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitorName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "VisitorPhone",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "VisitorId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Visitors",
                columns: new[] { "Id", "FullName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Іван Петренко", "+380501234567" },
                    { 2, "Олена Коваленко", "+380671234567" },
                    { 3, "Андрій Шевченко", "+380931234567" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VisitorId",
                table: "Bookings",
                column: "VisitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Visitors_VisitorId",
                table: "Bookings",
                column: "VisitorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
