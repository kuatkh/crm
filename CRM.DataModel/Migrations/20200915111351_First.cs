using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CRM.DataModel.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrmRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictCountries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictEnterprises",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<long>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictEnterprises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictEnterprises_EnterpriseBranches",
                        column: x => x.ParentId,
                        principalTable: "DictEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DictGenders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictIntolerances",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    DescriptionRu = table.Column<string>(nullable: true),
                    DescriptionKz = table.Column<string>(nullable: true),
                    DescriptionEn = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictIntolerances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrmRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmRoleClaims_CrmRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CrmRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmRoleClaims_CrmRoles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "CrmRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DictCities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    DictCountriesId = table.Column<long>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictCities_DictCountries",
                        column: x => x.DictCountriesId,
                        principalTable: "DictCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DictDepartments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    DictEnterprisesId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictDepartments_DictEnterprises",
                        column: x => x.DictEnterprisesId,
                        principalTable: "DictEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DictLoyaltyPrograms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    DescriptionRu = table.Column<string>(nullable: true),
                    DescriptionKz = table.Column<string>(nullable: true),
                    DescriptionEn = table.Column<string>(nullable: true),
                    DiscountAmount = table.Column<float>(nullable: false),
                    DictEnterprisesId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictLoyaltyPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictLoyaltyPrograms_DictEnterprises",
                        column: x => x.DictEnterprisesId,
                        principalTable: "DictEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DictPositions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    DescriptionRu = table.Column<string>(nullable: true),
                    DescriptionKz = table.Column<string>(nullable: true),
                    DescriptionEn = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    DictEnterprisesId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictPositions_DictEnterprises",
                        column: x => x.DictEnterprisesId,
                        principalTable: "DictEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DictServices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    DescriptionRu = table.Column<string>(nullable: true),
                    DescriptionKz = table.Column<string>(nullable: true),
                    DescriptionEn = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    DictDepartmentsId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictServices_DictDepartments",
                        column: x => x.DictDepartmentsId,
                        principalTable: "DictDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmEmployees",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrmUsersId = table.Column<long>(nullable: true),
                    Iin = table.Column<string>(nullable: true),
                    DocumentNumber = table.Column<string>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    SurnameRu = table.Column<string>(nullable: true),
                    SurnameKz = table.Column<string>(nullable: true),
                    SurnameEn = table.Column<string>(nullable: true),
                    MiddlenameRu = table.Column<string>(nullable: true),
                    MiddlenameKz = table.Column<string>(nullable: true),
                    MiddlenameEn = table.Column<string>(nullable: true),
                    AboutMe = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    DictGendersId = table.Column<long>(nullable: true),
                    DictEnterprisesId = table.Column<long>(nullable: true),
                    DictDepartmentsId = table.Column<long>(nullable: true),
                    DictPositionsId = table.Column<long>(nullable: true),
                    DictCitiesId = table.Column<long>(nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    PhotoB64 = table.Column<string>(nullable: true),
                    PhotoPath = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmEmployees_DictCities",
                        column: x => x.DictCitiesId,
                        principalTable: "DictCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmEmployees_DictDepartments",
                        column: x => x.DictDepartmentsId,
                        principalTable: "DictDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmEmployees_DictEnterprises",
                        column: x => x.DictEnterprisesId,
                        principalTable: "DictEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmEmployees_DictGenders",
                        column: x => x.DictGendersId,
                        principalTable: "DictGenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmEmployees_DictPositions",
                        column: x => x.DictPositionsId,
                        principalTable: "DictPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmEmployeesWorkPlans",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrmEmployeesId = table.Column<long>(nullable: false),
                    WorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    WorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    MondayWorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    MondayWorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    TuesdayWorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    TuesdayWorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    WebnesdayWorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    WebnesdayWorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    ThursdayWorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    ThursdayWorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    FridayWorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    FridayWorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    SaturdayWorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    SaturdayWorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    SundayWorkTimeFrom = table.Column<TimeSpan>(nullable: true),
                    SundayWorkTimeTo = table.Column<TimeSpan>(nullable: true),
                    WorkPeriodInDays = table.Column<int>(nullable: true),
                    DutyPeriodInDays = table.Column<int>(nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AuthorId = table.Column<long>(nullable: false),
                    EditorId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmEmployeesWorkPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmEmployeesWorkPlans_CrmEmployees_CrmEmployeesId",
                        column: x => x.CrmEmployeesId,
                        principalTable: "CrmEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrmPatientsAppointments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<long>(nullable: true),
                    CrmPatientsId = table.Column<long>(nullable: false),
                    ToCrmEmployeesId = table.Column<long>(nullable: false),
                    DictStatusesId = table.Column<long>(nullable: false),
                    AppointmentDateTime = table.Column<DateTime>(nullable: false),
                    Complain = table.Column<string>(nullable: true),
                    DoctorsAppointment = table.Column<string>(nullable: true),
                    ServicePrice = table.Column<float>(nullable: false),
                    IsOutOfLine = table.Column<bool>(nullable: true),
                    OutOfLineReason = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmPatientsAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmPatientsAppointments_DictStatuses",
                        column: x => x.DictStatusesId,
                        principalTable: "DictStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatientsAppointments_CrmPatientsAppointments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CrmPatientsAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatientsAppointments_CrmEmployees",
                        column: x => x.ToCrmEmployeesId,
                        principalTable: "CrmEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmPatientsAppointmentsFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrmPatientsAppointmentsId = table.Column<long>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmPatientsAppointmentsFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmPatientsAppointmentsFiles_CrmPatientsAppointments",
                        column: x => x.CrmPatientsAppointmentsId,
                        principalTable: "CrmPatientsAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmPatientsAppointmentsServices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrmPatientsAppointmentsId = table.Column<long>(nullable: false),
                    DictServicesId = table.Column<long>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmPatientsAppointmentsServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmPatientsAppointmentsServices_CrmPatientsAppointments",
                        column: x => x.CrmPatientsAppointmentsId,
                        principalTable: "CrmPatientsAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatientsAppointmentsServices_DictServices",
                        column: x => x.DictServicesId,
                        principalTable: "DictServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CrmEmployeesId = table.Column<long>(nullable: true),
                    CrmPatientsId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmUsers_CrmEmployees",
                        column: x => x.CrmEmployeesId,
                        principalTable: "CrmEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmHolidays",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HolidayDay = table.Column<int>(nullable: false),
                    HolidayMonth = table.Column<int>(nullable: false),
                    HolidayYear = table.Column<int>(nullable: true),
                    IsWork = table.Column<bool>(nullable: false),
                    IsRepeatYearly = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AuthorId = table.Column<long>(nullable: false),
                    EditorId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmHolidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmHolidays_Authors_CrmUsers",
                        column: x => x.AuthorId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmHolidays_Editor_CrmUsers",
                        column: x => x.EditorId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmPatients",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrmUsersId = table.Column<long>(nullable: true),
                    Iin = table.Column<string>(nullable: true),
                    DocumentNumber = table.Column<string>(nullable: true),
                    NameRu = table.Column<string>(nullable: true),
                    NameKz = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    SurnameRu = table.Column<string>(nullable: true),
                    SurnameKz = table.Column<string>(nullable: true),
                    SurnameEn = table.Column<string>(nullable: true),
                    MiddlenameRu = table.Column<string>(nullable: true),
                    MiddlenameKz = table.Column<string>(nullable: true),
                    MiddlenameEn = table.Column<string>(nullable: true),
                    AboutMe = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    JobPlace = table.Column<string>(nullable: true),
                    DictGendersId = table.Column<long>(nullable: true),
                    DictCitiesId = table.Column<long>(nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    PhotoB64 = table.Column<string>(nullable: true),
                    PhotoPath = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AuthorId = table.Column<long>(nullable: true),
                    EditorId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmPatients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmPatients_Author_CrmUsers",
                        column: x => x.AuthorId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatients_DictCities",
                        column: x => x.DictCitiesId,
                        principalTable: "DictCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatients_DictGenders",
                        column: x => x.DictGendersId,
                        principalTable: "DictGenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatients_Editor_CrmUsers",
                        column: x => x.EditorId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmUserClaims_CrmUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmUserClaims_CrmUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_CrmUserLogins_CrmUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmUserLogins_CrmUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmUserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    UserId1 = table.Column<long>(nullable: true),
                    RoleId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_CrmUserRoles_CrmRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CrmRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmUserRoles_CrmRoles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "CrmRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmUserRoles_CrmUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmUserRoles_CrmUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    UserId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_CrmUserTokens_CrmUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmUserTokens_CrmUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiverId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ReadDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationReceiver_CrmUsers",
                        column: x => x.ReceiverId,
                        principalTable: "CrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmPatientsIntolerances",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrmPatientsId = table.Column<long>(nullable: false),
                    DictIntolerancesId = table.Column<long>(nullable: true),
                    DescriptionRu = table.Column<string>(nullable: true),
                    DescriptionKz = table.Column<string>(nullable: true),
                    DescriptionEn = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmPatientsIntolerances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmPatientsIntolerances_CrmPatients",
                        column: x => x.CrmPatientsId,
                        principalTable: "CrmPatients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatientsIntolerances_DictIntolerances",
                        column: x => x.DictIntolerancesId,
                        principalTable: "DictIntolerances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmPatientsLoyaltyPrograms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrmPatientsId = table.Column<long>(nullable: false),
                    DictLoyaltyProgramsId = table.Column<long>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmPatientsLoyaltyPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmPatientsLoyaltyPrograms_CrmPatients",
                        column: x => x.CrmPatientsId,
                        principalTable: "CrmPatients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmPatientsLoyaltyPrograms_DictLoyaltyPrograms",
                        column: x => x.DictLoyaltyProgramsId,
                        principalTable: "DictLoyaltyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_DictCitiesId",
                table: "CrmEmployees",
                column: "DictCitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_DictDepartmentsId",
                table: "CrmEmployees",
                column: "DictDepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_DictEnterprisesId",
                table: "CrmEmployees",
                column: "DictEnterprisesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_DictGendersId",
                table: "CrmEmployees",
                column: "DictGendersId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployees_DictPositionsId",
                table: "CrmEmployees",
                column: "DictPositionsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployeesWorkPlans_AuthorId",
                table: "CrmEmployeesWorkPlans",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployeesWorkPlans_CrmEmployeesId",
                table: "CrmEmployeesWorkPlans",
                column: "CrmEmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmEmployeesWorkPlans_EditorId",
                table: "CrmEmployeesWorkPlans",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmHolidays_AuthorId",
                table: "CrmHolidays",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmHolidays_EditorId",
                table: "CrmHolidays",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatients_AuthorId",
                table: "CrmPatients",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatients_DictCitiesId",
                table: "CrmPatients",
                column: "DictCitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatients_DictGendersId",
                table: "CrmPatients",
                column: "DictGendersId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatients_EditorId",
                table: "CrmPatients",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointments_CrmPatientsId",
                table: "CrmPatientsAppointments",
                column: "CrmPatientsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointments_DictStatusesId",
                table: "CrmPatientsAppointments",
                column: "DictStatusesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointments_ParentId",
                table: "CrmPatientsAppointments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointments_ToCrmEmployeesId",
                table: "CrmPatientsAppointments",
                column: "ToCrmEmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointmentsFiles_CrmPatientsAppointmentsId",
                table: "CrmPatientsAppointmentsFiles",
                column: "CrmPatientsAppointmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointmentsServices_CrmPatientsAppointmentsId",
                table: "CrmPatientsAppointmentsServices",
                column: "CrmPatientsAppointmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsAppointmentsServices_DictServicesId",
                table: "CrmPatientsAppointmentsServices",
                column: "DictServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsIntolerances_CrmPatientsId",
                table: "CrmPatientsIntolerances",
                column: "CrmPatientsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsIntolerances_DictIntolerancesId",
                table: "CrmPatientsIntolerances",
                column: "DictIntolerancesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsLoyaltyPrograms_CrmPatientsId",
                table: "CrmPatientsLoyaltyPrograms",
                column: "CrmPatientsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmPatientsLoyaltyPrograms_DictLoyaltyProgramsId",
                table: "CrmPatientsLoyaltyPrograms",
                column: "DictLoyaltyProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmRoleClaims_RoleId",
                table: "CrmRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmRoleClaims_RoleId1",
                table: "CrmRoleClaims",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "CrmRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserClaims_UserId",
                table: "CrmUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserClaims_UserId1",
                table: "CrmUserClaims",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserLogins_UserId",
                table: "CrmUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserLogins_UserId1",
                table: "CrmUserLogins",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserRoles_RoleId",
                table: "CrmUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserRoles_RoleId1",
                table: "CrmUserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserRoles_UserId1",
                table: "CrmUserRoles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUsers_CrmEmployeesId",
                table: "CrmUsers",
                column: "CrmEmployeesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmUsers_CrmPatientsId",
                table: "CrmUsers",
                column: "CrmPatientsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "CrmUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "CrmUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserTokens_UserId1",
                table: "CrmUserTokens",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_DictCities_DictCountriesId",
                table: "DictCities",
                column: "DictCountriesId");

            migrationBuilder.CreateIndex(
                name: "IX_DictDepartments_DictEnterprisesId",
                table: "DictDepartments",
                column: "DictEnterprisesId");

            migrationBuilder.CreateIndex(
                name: "IX_DictEnterprises_ParentId",
                table: "DictEnterprises",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DictLoyaltyPrograms_DictEnterprisesId",
                table: "DictLoyaltyPrograms",
                column: "DictEnterprisesId");

            migrationBuilder.CreateIndex(
                name: "IX_DictPositions_DictEnterprisesId",
                table: "DictPositions",
                column: "DictEnterprisesId");

            migrationBuilder.CreateIndex(
                name: "IX_DictServices_DictDepartmentsId",
                table: "DictServices",
                column: "DictDepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrmSystemSettings_Authors_CrmUsers",
                table: "CrmEmployeesWorkPlans",
                column: "AuthorId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmSystemSettings_Editor_CrmUsers",
                table: "CrmEmployeesWorkPlans",
                column: "EditorId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmPatientsAppointments_CrmPatients",
                table: "CrmPatientsAppointments",
                column: "CrmPatientsId",
                principalTable: "CrmPatients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmUsers_CrmPatients",
                table: "CrmUsers",
                column: "CrmPatientsId",
                principalTable: "CrmPatients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_DictCities",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmPatients_DictCities",
                table: "CrmPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_DictDepartments",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_DictEnterprises",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_DictPositions_DictEnterprises",
                table: "DictPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_DictGenders",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmPatients_DictGenders",
                table: "CrmPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmEmployees_DictPositions",
                table: "CrmEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmPatients_Author_CrmUsers",
                table: "CrmPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmPatients_Editor_CrmUsers",
                table: "CrmPatients");

            migrationBuilder.DropTable(
                name: "CrmEmployeesWorkPlans");

            migrationBuilder.DropTable(
                name: "CrmHolidays");

            migrationBuilder.DropTable(
                name: "CrmPatientsAppointmentsFiles");

            migrationBuilder.DropTable(
                name: "CrmPatientsAppointmentsServices");

            migrationBuilder.DropTable(
                name: "CrmPatientsIntolerances");

            migrationBuilder.DropTable(
                name: "CrmPatientsLoyaltyPrograms");

            migrationBuilder.DropTable(
                name: "CrmRoleClaims");

            migrationBuilder.DropTable(
                name: "CrmUserClaims");

            migrationBuilder.DropTable(
                name: "CrmUserLogins");

            migrationBuilder.DropTable(
                name: "CrmUserRoles");

            migrationBuilder.DropTable(
                name: "CrmUserTokens");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "CrmPatientsAppointments");

            migrationBuilder.DropTable(
                name: "DictServices");

            migrationBuilder.DropTable(
                name: "DictIntolerances");

            migrationBuilder.DropTable(
                name: "DictLoyaltyPrograms");

            migrationBuilder.DropTable(
                name: "CrmRoles");

            migrationBuilder.DropTable(
                name: "DictStatuses");

            migrationBuilder.DropTable(
                name: "DictCities");

            migrationBuilder.DropTable(
                name: "DictCountries");

            migrationBuilder.DropTable(
                name: "DictDepartments");

            migrationBuilder.DropTable(
                name: "DictEnterprises");

            migrationBuilder.DropTable(
                name: "DictGenders");

            migrationBuilder.DropTable(
                name: "DictPositions");

            migrationBuilder.DropTable(
                name: "CrmUsers");

            migrationBuilder.DropTable(
                name: "CrmEmployees");

            migrationBuilder.DropTable(
                name: "CrmPatients");
        }
    }
}
