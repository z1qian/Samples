using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._6_关系配置.Migrations
{
    /// <inheritdoc />
    public partial class ArticleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Comments_T_Articles_ActicleId",
                table: "T_Comments");

            migrationBuilder.RenameColumn(
                name: "ActicleId",
                table: "T_Comments",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Comments_ActicleId",
                table: "T_Comments",
                newName: "IX_T_Comments_ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Comments_T_Articles_ArticleId",
                table: "T_Comments",
                column: "ArticleId",
                principalTable: "T_Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Comments_T_Articles_ArticleId",
                table: "T_Comments");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "T_Comments",
                newName: "ActicleId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Comments_ArticleId",
                table: "T_Comments",
                newName: "IX_T_Comments_ActicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Comments_T_Articles_ActicleId",
                table: "T_Comments",
                column: "ActicleId",
                principalTable: "T_Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
