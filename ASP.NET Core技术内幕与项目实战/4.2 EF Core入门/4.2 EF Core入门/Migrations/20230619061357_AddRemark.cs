using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._2_EF_Core入门.Migrations
{
    /// <inheritdoc />
    public partial class AddRemark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "T_Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "T_Books");
        }
    }
}
