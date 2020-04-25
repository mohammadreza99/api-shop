using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApi.DataLayer.Migrations
{
    public partial class number2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Discount");

            migrationBuilder.AlterColumn<string>(
                name: "ColorCode",
                table: "FeatureValue",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Discount",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Discount");

            migrationBuilder.AlterColumn<int>(
                name: "ColorCode",
                table: "FeatureValue",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Discount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
