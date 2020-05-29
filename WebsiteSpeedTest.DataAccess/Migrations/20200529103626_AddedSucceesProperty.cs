﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteSpeedTest.DataAccess.Migrations
{
    public partial class AddedSucceesProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "Endpoint",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Success",
                table: "Endpoint");
        }
    }
}
