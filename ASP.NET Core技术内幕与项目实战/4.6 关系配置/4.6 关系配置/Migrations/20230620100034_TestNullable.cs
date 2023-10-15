using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._6_关系配置.Migrations
{
    /// <inheritdoc />
    public partial class TestNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Comments_T_Articles_ActicleId",
                table: "T_Comments");

            migrationBuilder.AlterColumn<long>(
                name: "ActicleId",
                table: "T_Comments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Comments_T_Articles_ActicleId",
                table: "T_Comments",
                column: "ActicleId",
                principalTable: "T_Articles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Comments_T_Articles_ActicleId",
                table: "T_Comments");

            migrationBuilder.AlterColumn<long>(
                name: "ActicleId",
                table: "T_Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
