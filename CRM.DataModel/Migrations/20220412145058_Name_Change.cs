using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataModel.Migrations
{
    public partial class Name_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictStatuses");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "DictStatuses");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "DictServices");

            migrationBuilder.DropColumn(
                name: "DescriptionKz",
                table: "DictServices");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "DictServices");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictServices");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "DictPositions");

            migrationBuilder.DropColumn(
                name: "DescriptionKz",
                table: "DictPositions");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "DictPositions");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictPositions");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "DictLoyaltyPrograms");

            migrationBuilder.DropColumn(
                name: "DescriptionKz",
                table: "DictLoyaltyPrograms");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "DictLoyaltyPrograms");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictLoyaltyPrograms");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "DictIntolerances");

            migrationBuilder.DropColumn(
                name: "DescriptionKz",
                table: "DictIntolerances");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "DictIntolerances");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictIntolerances");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictGenders");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "DictGenders");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictEnterprises");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "DictEnterprises");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictDepartments");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "DictDepartments");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictCountries");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "DictCountries");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DictCities");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "DictCities");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "CrmPatientsIntolerances");

            migrationBuilder.DropColumn(
                name: "DescriptionKz",
                table: "CrmPatientsIntolerances");

            migrationBuilder.DropColumn(
                name: "MiddlenameEn",
                table: "CrmPatients");

            migrationBuilder.DropColumn(
                name: "MiddlenameKz",
                table: "CrmPatients");

            migrationBuilder.DropColumn(
                name: "MiddlenameRu",
                table: "CrmPatients");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "CrmPatients");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "CrmPatients");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "CrmPatients");

            migrationBuilder.DropColumn(
                name: "MiddlenameEn",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "MiddlenameKz",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "MiddlenameRu",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "NameKz",
                table: "CrmEmployees");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "CrmEmployees");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictServices",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameKz",
                table: "DictServices",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictPositions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameKz",
                table: "DictPositions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictLoyaltyPrograms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameKz",
                table: "DictLoyaltyPrograms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictIntolerances",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameKz",
                table: "DictIntolerances",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictGenders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictEnterprises",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictDepartments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictCountries",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "DictCities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionRu",
                table: "CrmPatientsIntolerances",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "SurnameRu",
                table: "CrmPatients",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "SurnameKz",
                table: "CrmPatients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SurnameEn",
                table: "CrmPatients",
                newName: "Middlename");

            migrationBuilder.RenameColumn(
                name: "SurnameRu",
                table: "CrmEmployees",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "SurnameKz",
                table: "CrmEmployees",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SurnameEn",
                table: "CrmEmployees",
                newName: "Middlename");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDateTime",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictStatuses",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictStatuses",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictStatuses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictServices",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictServices",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictServices",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictPositions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictPositions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictPositions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictLoyaltyPrograms",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictLoyaltyPrograms",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictLoyaltyPrograms",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictIntolerances",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictIntolerances",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictIntolerances",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictGenders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictGenders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictGenders",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictEnterprises",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictEnterprises",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictEnterprises",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictDepartments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictDepartments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictDepartments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictCountries",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictCountries",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictCountries",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictCities",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictCities",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictCities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmUsers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmUsers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmUsers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsLoyaltyPrograms",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsLoyaltyPrograms",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsLoyaltyPrograms",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsIntolerances",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsIntolerances",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsIntolerances",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsAppointmentsServices",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsAppointmentsServices",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsAppointmentsServices",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsAppointmentsFiles",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsAppointmentsFiles",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsAppointmentsFiles",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentStartedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentStartDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentEndedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentEndDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatients",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatients",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatients",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmHolidays",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmHolidays",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmHolidays",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmEmployeesWorkPlans",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmEmployeesWorkPlans",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmEmployeesWorkPlans",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmEmployees",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmEmployees",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmEmployees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictStatuses",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictServices",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DictServices",
                newName: "NameKz");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictPositions",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DictPositions",
                newName: "NameKz");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictLoyaltyPrograms",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DictLoyaltyPrograms",
                newName: "NameKz");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictIntolerances",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DictIntolerances",
                newName: "NameKz");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictGenders",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictEnterprises",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictDepartments",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictCountries",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DictCities",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CrmPatientsIntolerances",
                newName: "DescriptionRu");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "CrmPatients",
                newName: "SurnameRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CrmPatients",
                newName: "SurnameKz");

            migrationBuilder.RenameColumn(
                name: "Middlename",
                table: "CrmPatients",
                newName: "SurnameEn");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "CrmEmployees",
                newName: "SurnameRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CrmEmployees",
                newName: "SurnameKz");

            migrationBuilder.RenameColumn(
                name: "Middlename",
                table: "CrmEmployees",
                newName: "SurnameEn");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDateTime",
                table: "Notifications",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Notifications",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Notifications",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictStatuses",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictStatuses",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictStatuses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictStatuses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "DictStatuses",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictServices",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictServices",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictServices",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "DictServices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionKz",
                table: "DictServices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "DictServices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictServices",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictPositions",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictPositions",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictPositions",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "DictPositions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionKz",
                table: "DictPositions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "DictPositions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictPositions",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictLoyaltyPrograms",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictLoyaltyPrograms",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictLoyaltyPrograms",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "DictLoyaltyPrograms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionKz",
                table: "DictLoyaltyPrograms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "DictLoyaltyPrograms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictLoyaltyPrograms",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictIntolerances",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictIntolerances",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictIntolerances",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "DictIntolerances",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionKz",
                table: "DictIntolerances",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "DictIntolerances",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictIntolerances",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictGenders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictGenders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictGenders",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictGenders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "DictGenders",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictEnterprises",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictEnterprises",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictEnterprises",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictEnterprises",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "DictEnterprises",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictDepartments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictDepartments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictDepartments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictDepartments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "DictDepartments",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictCountries",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictCountries",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictCountries",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictCountries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "DictCountries",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "DictCities",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "DictCities",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DictCities",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DictCities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "DictCities",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmUsers",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmUsers",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmUsers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsLoyaltyPrograms",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsLoyaltyPrograms",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsLoyaltyPrograms",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsIntolerances",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsIntolerances",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsIntolerances",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "CrmPatientsIntolerances",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionKz",
                table: "CrmPatientsIntolerances",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsAppointmentsServices",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsAppointmentsServices",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsAppointmentsServices",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsAppointmentsFiles",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsAppointmentsFiles",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsAppointmentsFiles",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentStartedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentStartDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentEndedDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentEndDateTime",
                table: "CrmPatientsAppointments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmPatients",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmPatients",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmPatients",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "MiddlenameEn",
                table: "CrmPatients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddlenameKz",
                table: "CrmPatients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddlenameRu",
                table: "CrmPatients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "CrmPatients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "CrmPatients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "CrmPatients",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmHolidays",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmHolidays",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmHolidays",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmEmployeesWorkPlans",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmEmployeesWorkPlans",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmEmployeesWorkPlans",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedDateTime",
                table: "CrmEmployees",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDateTime",
                table: "CrmEmployees",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CrmEmployees",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "MiddlenameEn",
                table: "CrmEmployees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddlenameKz",
                table: "CrmEmployees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddlenameRu",
                table: "CrmEmployees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "CrmEmployees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKz",
                table: "CrmEmployees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "CrmEmployees",
                type: "text",
                nullable: true);
        }
    }
}
