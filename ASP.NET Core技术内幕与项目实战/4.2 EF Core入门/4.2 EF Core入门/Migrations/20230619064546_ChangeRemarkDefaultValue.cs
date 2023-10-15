using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._2_EF_Core入门.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRemarkDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "T_Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "暂无备注",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldDefaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "T_Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldDefaultValue: "暂无备注");
        }
    }
}
