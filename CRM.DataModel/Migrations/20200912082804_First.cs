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
                name: "DictEnterpirses",
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
                    table.PrimaryKey("PK_DictEnterpirses", x => x.Id);
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
                        name: "FK_DictDepartments_DictEnterpirses",
                        column: x => x.DictEnterprisesId,
                        principalTable: "DictEnterpirses",
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
                    DictEnterprisesId = table.Column<long>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictPositions_DictEnterpirses",
                        column: x => x.DictEnterprisesId,
                        principalTable: "DictEnterpirses",
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
                    Iin = table.Column<string>(nullable: true),
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
                    JobPlace = table.Column<string>(nullable: true),
                    DictGendersId = table.Column<long>(nullable: true),
                    DictEnterprisesId = table.Column<long>(nullable: true),
                    DictDepartmentsId = table.Column<long>(nullable: true),
                    DictPositionsId = table.Column<long>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    PhotoB64 = table.Column<string>(nullable: true),
                    PhotoPath = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: true),
                    DeletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmUsers_DictDepartments",
                        column: x => x.DictDepartmentsId,
                        principalTable: "DictDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmUsers_DictEnterpirses",
                        column: x => x.DictEnterprisesId,
                        principalTable: "DictEnterpirses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmUsers_DictGenders",
                        column: x => x.DictGendersId,
                        principalTable: "DictGenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmUsers_DictPositions",
                        column: x => x.DictPositionsId,
                        principalTable: "DictPositions",
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
                name: "IX_CrmUsers_DictDepartmentsId",
                table: "CrmUsers",
                column: "DictDepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUsers_DictEnterprisesId",
                table: "CrmUsers",
                column: "DictEnterprisesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUsers_DictGendersId",
                table: "CrmUsers",
                column: "DictGendersId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmUsers_DictPositionsId",
                table: "CrmUsers",
                column: "DictPositionsId");

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
                name: "IX_DictDepartments_DictEnterprisesId",
                table: "DictDepartments",
                column: "DictEnterprisesId");

            migrationBuilder.CreateIndex(
                name: "IX_DictPositions_DictEnterprisesId",
                table: "DictPositions",
                column: "DictEnterprisesId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "CrmRoles");

            migrationBuilder.DropTable(
                name: "CrmUsers");

            migrationBuilder.DropTable(
                name: "DictDepartments");

            migrationBuilder.DropTable(
                name: "DictGenders");

            migrationBuilder.DropTable(
                name: "DictPositions");

            migrationBuilder.DropTable(
                name: "DictEnterpirses");
        }
    }
}
