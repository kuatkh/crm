using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.DataModel.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrmUserRoles_CrmUsers_UserId",
                table: "CrmUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmUserRoles_CrmUsers_UserId1",
                table: "CrmUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_CrmUserRoles_UserId1",
                table: "CrmUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "CrmUserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_CrmUsers_UserRoles",
                table: "CrmUserRoles",
                column: "UserId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrmUsers_UserRoles",
                table: "CrmUserRoles");

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "CrmUserRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmUserRoles_UserId1",
                table: "CrmUserRoles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CrmUserRoles_CrmUsers_UserId",
                table: "CrmUserRoles",
                column: "UserId",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmUserRoles_CrmUsers_UserId1",
                table: "CrmUserRoles",
                column: "UserId1",
                principalTable: "CrmUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
