using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Migrations
{
    /// <inheritdoc />
    public partial class edit_logs_criteria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_modefiedById",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Patients_PatientID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Receptionists_ReseptID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Therapists_TherapistID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Therapists_AppUserId",
                table: "Therapists");

            migrationBuilder.DropIndex(
                name: "IX_Receptionists_AppUserId",
                table: "Receptionists");

            migrationBuilder.DropIndex(
                name: "IX_Patients_AppUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_modefiedById",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PatientID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReseptID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TherapistID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Lastmodified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RequirePasswordChange",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReseptID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TherapistID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "isLocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "modefiedById",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "TreatmentPlans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "TreatmentPlanExercises",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "TherapySessions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Therapists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Therapists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Lastmodified",
                table: "Therapists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LockCount",
                table: "Therapists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                table: "Therapists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedbyId",
                table: "Therapists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePasswordChange",
                table: "Therapists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Therapists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isLocked",
                table: "Therapists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "modefiedById",
                table: "Therapists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Receptionists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Receptionists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Lastmodified",
                table: "Receptionists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LockCount",
                table: "Receptionists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                table: "Receptionists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedbyId",
                table: "Receptionists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePasswordChange",
                table: "Receptionists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Receptionists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isLocked",
                table: "Receptionists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "modefiedById",
                table: "Receptionists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Plans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Payments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Lastmodified",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LockCount",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePasswordChange",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isLocked",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "modefiedById",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Muscles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "MedicalServices",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Exercises",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "ExerciseCategories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Equipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Appointments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_AppUserId",
                table: "Therapists",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_CreatedById",
                table: "Therapists",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_ModifiedbyId",
                table: "Therapists",
                column: "ModifiedbyId");

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_AppUserId",
                table: "Receptionists",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_CreatedById",
                table: "Receptionists",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_ModifiedbyId",
                table: "Receptionists",
                column: "ModifiedbyId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AppUserId",
                table: "Patients",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CreatedById",
                table: "Patients",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_modefiedById",
                table: "Patients",
                column: "modefiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_modefiedById",
                table: "Patients",
                column: "modefiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receptionists_AspNetUsers_CreatedById",
                table: "Receptionists",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receptionists_AspNetUsers_ModifiedbyId",
                table: "Receptionists",
                column: "ModifiedbyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Therapists_AspNetUsers_CreatedById",
                table: "Therapists",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Therapists_AspNetUsers_ModifiedbyId",
                table: "Therapists",
                column: "ModifiedbyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_modefiedById",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Receptionists_AspNetUsers_CreatedById",
                table: "Receptionists");

            migrationBuilder.DropForeignKey(
                name: "FK_Receptionists_AspNetUsers_ModifiedbyId",
                table: "Receptionists");

            migrationBuilder.DropForeignKey(
                name: "FK_Therapists_AspNetUsers_CreatedById",
                table: "Therapists");

            migrationBuilder.DropForeignKey(
                name: "FK_Therapists_AspNetUsers_ModifiedbyId",
                table: "Therapists");

            migrationBuilder.DropIndex(
                name: "IX_Therapists_AppUserId",
                table: "Therapists");

            migrationBuilder.DropIndex(
                name: "IX_Therapists_CreatedById",
                table: "Therapists");

            migrationBuilder.DropIndex(
                name: "IX_Therapists_ModifiedbyId",
                table: "Therapists");

            migrationBuilder.DropIndex(
                name: "IX_Receptionists_AppUserId",
                table: "Receptionists");

            migrationBuilder.DropIndex(
                name: "IX_Receptionists_CreatedById",
                table: "Receptionists");

            migrationBuilder.DropIndex(
                name: "IX_Receptionists_ModifiedbyId",
                table: "Receptionists");

            migrationBuilder.DropIndex(
                name: "IX_Patients_AppUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CreatedById",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_modefiedById",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "Lastmodified",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "LockCount",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "LockDate",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "ModifiedbyId",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "RequirePasswordChange",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "isLocked",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "modefiedById",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "Lastmodified",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "LockCount",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "LockDate",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "ModifiedbyId",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "RequirePasswordChange",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "isLocked",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "modefiedById",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Lastmodified",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LockCount",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LockDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "RequirePasswordChange",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "isLocked",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "modefiedById",
                table: "Patients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "TreatmentPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "TreatmentPlanExercises",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "TherapySessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Plans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Muscles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "MedicalServices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Exercises",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "ExerciseCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Equipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Lastmodified",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LockCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePasswordChange",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReseptID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TherapistID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isLocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "modefiedById",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lastmodified",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_AppUserId",
                table: "Therapists",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_AppUserId",
                table: "Receptionists",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AppUserId",
                table: "Patients",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedById",
                table: "AspNetUsers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_modefiedById",
                table: "AspNetUsers",
                column: "modefiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PatientID",
                table: "AspNetUsers",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReseptID",
                table: "AspNetUsers",
                column: "ReseptID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TherapistID",
                table: "AspNetUsers",
                column: "TherapistID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedById",
                table: "AspNetUsers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_modefiedById",
                table: "AspNetUsers",
                column: "modefiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Patients_PatientID",
                table: "AspNetUsers",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Receptionists_ReseptID",
                table: "AspNetUsers",
                column: "ReseptID",
                principalTable: "Receptionists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Therapists_TherapistID",
                table: "AspNetUsers",
                column: "TherapistID",
                principalTable: "Therapists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
