﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteSpeedTest.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestBenchmarkEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestBenchmarkEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Endpoint",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uri = table.Column<string>(nullable: true),
                    ResponseTime = table.Column<int>(nullable: false),
                    StatusCode = table.Column<int>(nullable: false),
                    RequestBenchmarkEntryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endpoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Endpoint_RequestBenchmarkEntry_RequestBenchmarkEntryId",
                        column: x => x.RequestBenchmarkEntryId,
                        principalTable: "RequestBenchmarkEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Endpoint_RequestBenchmarkEntryId",
                table: "Endpoint",
                column: "RequestBenchmarkEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Endpoint");

            migrationBuilder.DropTable(
                name: "RequestBenchmarkEntry");
        }
    }
}
