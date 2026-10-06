using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Migrations
{
    /// <inheritdoc />
    public partial class edit_equipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Plans_planID",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "MerchantName",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "Equipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "planID",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Plans_planID",
                table: "Appointments",
                column: "planID",
                principalTable: "Plans",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Plans_planID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MerchantName",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Equipments");

            migrationBuilder.AlterColumn<int>(
                name: "planID",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Plans_planID",
                table: "Appointments",
                column: "planID",
                principalTable: "Plans",
                principalColumn: "ID");
        }
    }
}
