﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockStuff.Migrations
{
    /// <inheritdoc />
    public partial class last_fetched_at : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchedAt",
                table: "Feeds",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchedAt",
                table: "Feeds");
        }
    }
}
