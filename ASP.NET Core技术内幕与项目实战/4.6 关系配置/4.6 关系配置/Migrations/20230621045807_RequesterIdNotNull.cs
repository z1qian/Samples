using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._6_关系配置.Migrations
{
    /// <inheritdoc />
    public partial class RequesterIdNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Leaves_T_Users_RequesterId",
                table: "T_Leaves");

            migrationBuilder.AlterColumn<long>(
                name: "RequesterId",
                table: "T_Leaves",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Leaves_T_Users_RequesterId",
                table: "T_Leaves",
                column: "RequesterId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Leaves_T_Users_RequesterId",
                table: "T_Leaves");

            migrationBuilder.AlterColumn<long>(
                name: "RequesterId",
                table: "T_Leaves",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Leaves_T_Users_RequesterId",
                table: "T_Leaves",
                column: "RequesterId",
                principalTable: "T_Users",
                principalColumn: "Id");
        }
    }
}
