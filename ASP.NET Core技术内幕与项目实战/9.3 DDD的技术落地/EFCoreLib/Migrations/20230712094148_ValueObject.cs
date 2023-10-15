using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreLib.Migrations
{
    /// <inheritdoc />
    public partial class ValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Chinese = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name_English = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Area_Value = table.Column<double>(type: "float", nullable: false),
                    Area_Unit = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Level = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: true),
                    Location_Longitude = table.Column<double>(type: "float", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Credit = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
