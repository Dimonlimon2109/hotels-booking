using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelsBooking.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class DecompositionAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Hotels",
                newName: "Street");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Hotels",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1500)",
                oldMaxLength: 1500);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Hotels",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Hotels",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Hotels",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Hotels",
                newName: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Hotels",
                type: "character varying(1500)",
                maxLength: 1500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);
        }
    }
}
