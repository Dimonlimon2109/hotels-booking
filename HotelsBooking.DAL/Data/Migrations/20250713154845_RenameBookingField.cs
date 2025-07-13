using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelsBooking.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameBookingField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentIntentId",
                table: "Bookings",
                newName: "ChargeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChargeId",
                table: "Bookings",
                newName: "PaymentIntentId");
        }
    }
}
