﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace EventsExpress.Db.Migrations
{
    public partial class GoogleOAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "oauthIssuer",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "oauthSubject",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oauthIssuer",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "oauthSubject",
                table: "Users");
        }
    }
}
