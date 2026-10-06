using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Migrations
{
    /// <inheritdoc />
    public partial class edit_payment_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MedicalServiceID",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "feesOnMedicalService",
                table: "Appointments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "feesOnpatient",
                table: "Appointments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MedicalServiceID",
                table: "Payments",
                column: "MedicalServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PatientID",
                table: "Payments",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_MedicalServices_MedicalServiceID",
                table: "Payments",
                column: "MedicalServiceID",
                principalTable: "MedicalServices",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Patients_PatientID",
                table: "Payments",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_MedicalServices_MedicalServiceID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Patients_PatientID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_MedicalServiceID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PatientID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "MedicalServiceID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "feesOnMedicalService",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "feesOnpatient",
                table: "Appointments");
        }
    }
}
