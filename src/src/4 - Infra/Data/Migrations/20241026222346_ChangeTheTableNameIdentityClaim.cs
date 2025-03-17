using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApiV8.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTheTableNameIdentityClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityClaim_IdentityUser_UserId",
                table: "IdentityClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityClaim",
                table: "IdentityClaim");

            migrationBuilder.RenameTable(
                name: "IdentityClaim",
                newName: "IdentityUserClaim");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityClaim_UserId",
                table: "IdentityUserClaim",
                newName: "IX_IdentityUserClaim_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim",
                table: "IdentityUserClaim",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim_IdentityUser_UserId",
                table: "IdentityUserClaim",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserClaim_IdentityUser_UserId",
                table: "IdentityUserClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim",
                table: "IdentityUserClaim");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim",
                newName: "IdentityClaim");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserClaim_UserId",
                table: "IdentityClaim",
                newName: "IX_IdentityClaim_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityClaim",
                table: "IdentityClaim",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityClaim_IdentityUser_UserId",
                table: "IdentityClaim",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
