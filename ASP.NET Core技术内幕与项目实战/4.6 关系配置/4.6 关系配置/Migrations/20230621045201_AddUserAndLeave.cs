﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._6_关系配置.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndLeave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Leaves",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterId = table.Column<long>(type: "bigint", nullable: true),
                    ApproverId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Leaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Leaves_T_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "T_Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_Leaves_T_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "T_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Leaves_ApproverId",
                table: "T_Leaves",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Leaves_RequesterId",
                table: "T_Leaves",
                column: "RequesterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Leaves");

            migrationBuilder.DropTable(
                name: "T_Users");
        }
    }
}
