using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class virtion3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelivaryFee",
                table: "Resturant");

            migrationBuilder.DropColumn(
                name: "DelivaryTime",
                table: "Resturant");

       
            migrationBuilder.AddColumn<float>(
                name: "DelivaryFee",
                table: "ResturantCities",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "DelivaryTime",
                table: "ResturantCities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelivaryFee",
                table: "ResturantCities");

            migrationBuilder.DropColumn(
                name: "DelivaryTime",
                table: "ResturantCities");

            migrationBuilder.AddColumn<float>(
                name: "DelivaryFee",
                table: "Resturant",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "DelivaryTime",
                table: "Resturant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

          
        }
    }
}
