using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockStuff.Migrations
{
    /// <inheritdoc />
    public partial class Add_Subscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_AppUsers_AppUserId",
                table: "Feeds");

            migrationBuilder.DropIndex(
                name: "IX_Feeds_AppUserId",
                table: "Feeds");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Feeds");

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FeedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_Feeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "Feeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_AppUserId",
                table: "Subscription",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_FeedId",
                table: "Subscription",
                column: "FeedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Feeds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Feeds_AppUserId",
                table: "Feeds",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_AppUsers_AppUserId",
                table: "Feeds",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
