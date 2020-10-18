using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.DataModel.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDateTime",
                table: "CrmPatientsAppointments");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "DictPositions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentEndDateTime",
                table: "CrmPatientsAppointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentEndedDateTime",
                table: "CrmPatientsAppointments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentStartDateTime",
                table: "CrmPatientsAppointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentStartedDateTime",
                table: "CrmPatientsAppointments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "CrmPatientsAppointments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CrmPatientsAppointments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EditorId",
                table: "CrmPatientsAppointments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "CrmEmployees",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CrmPatientsId",
                table: "CrmEmployees",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EditorId",
                table: "CrmEmployees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointments_AuthorId",
                table: "CrmPatientsAppointments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointments_EditorId",
                table: "CrmPatientsAppointments",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_AuthorId",
                table: "CrmEmployees",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_CrmPatientsId",
                table: "CrmEmployees",
                column: "CrmPatientsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_EditorId",
                table: "CrmEmployees",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrmEmployees_CrmUsers_AuthorId",
                table: "CrmEmployees",
                column: "AuthorId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmEmployees_CrmPatients_CrmPatientsId",
                table: "CrmEmployees",
                column: "CrmPatientsId",
                principalTable: "CrmPatients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmEmployees_CrmUsers_EditorId",
                table: "CrmEmployees",
                column: "EditorId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmPatientsAppointments_Author_CrmUsers",
                table: "CrmPatientsAppointments",
                column: "AuthorId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmPatientsAppointments_Editor_CrmUsers",
                table: "CrmPatientsAppointments",
                column: "EditorId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_CrmUsers_AuthorId",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_CrmPatients_CrmPatientsId",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_CrmUsers_EditorId",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmPatientsAppointments_Author_CrmUsers",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmPatientsAppointments_Editor_CrmUsers",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropIndex(
                name: "IX_CrmPatientsAppointments_AuthorId",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropIndex(
                name: "IX_CrmPatientsAppointments_EditorId",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropIndex(
                name: "IX_CrmEmployees_AuthorId",
                table: "CrmEmployees");

            migrationBuilder.DropIndex(
                name: "IX_CrmEmployees_CrmPatientsId",
                table: "CrmEmployees");

            migrationBuilder.DropIndex(
                name: "IX_CrmEmployees_EditorId",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "DictPositions");

            migrationBuilder.DropColumn(
                name: "AppointmentEndDateTime",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropColumn(
                name: "AppointmentEndedDateTime",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropColumn(
                name: "AppointmentStartDateTime",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropColumn(
                name: "AppointmentStartedDateTime",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "CrmPatientsAppointments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "CrmPatientsId",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "CrmEmployees");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
