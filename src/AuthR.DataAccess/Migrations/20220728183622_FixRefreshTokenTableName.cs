using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthR.DataAccess.Migrations
{
    public partial class FixRefreshTokenTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken1",
                table: "RefreshToken1");

            migrationBuilder.RenameTable(
                name: "RefreshToken1",
                newName: "RefreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken1_Guid",
                table: "RefreshToken",
                newName: "IX_RefreshToken_Guid");

            migrationBuilder.AddColumn<DateTime>(
                name: "Revoked",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "RefreshToken1");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_Guid",
                table: "RefreshToken1",
                newName: "IX_RefreshToken1_Guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken1",
                table: "RefreshToken1",
                column: "Id");
        }
    }
}
