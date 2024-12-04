using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class initUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pitch_PitchesType_IdPitchType",
                table: "Pitch");

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasCheckedIn = table.Column<bool>(type: "bit", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IdCustomer = table.Column<int>(type: "int", nullable: false),
                    IdPitch = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Customer_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Booking_Pitch_IdPitch",
                        column: x => x.IdPitch,
                        principalTable: "Pitch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$L.hBYu5G04H5TTIQQpi.Uu/4TflNehXeSP5VqhsPPuLoXla8jkzoS");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_IdCustomer",
                table: "Booking",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_IdPitch",
                table: "Booking",
                column: "IdPitch");

            migrationBuilder.AddForeignKey(
                name: "FK_Pitch_PitchesType_IdPitchType",
                table: "Pitch",
                column: "IdPitchType",
                principalTable: "PitchesType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pitch_PitchesType_IdPitchType",
                table: "Pitch");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TiirXeau5Igyk1cKv.hB8uo40tNaqfOiWfveLeoHdZ9fEIYOkGjpO");

            migrationBuilder.AddForeignKey(
                name: "FK_Pitch_PitchesType_IdPitchType",
                table: "Pitch",
                column: "IdPitchType",
                principalTable: "PitchesType",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
