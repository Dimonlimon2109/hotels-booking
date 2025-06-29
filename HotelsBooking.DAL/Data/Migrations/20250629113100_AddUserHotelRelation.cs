using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelsBooking.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserHotelRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Hotels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_OwnerId",
                table: "Hotels",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Users_OwnerId",
                table: "Hotels",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Users_OwnerId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_OwnerId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Hotels");
        }
    }
}
