using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockStuff.Migrations
{
    /// <inheritdoc />
    public partial class Add_Subscriptions2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_AppUsers_AppUserId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Feeds_FeedId",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_FeedId",
                table: "Subscription");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "Subscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_AppUserId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_FeedId_AppUserId",
                table: "Subscriptions",
                columns: new[] { "FeedId", "AppUserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AppUsers_AppUserId",
                table: "Subscriptions",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Feeds_FeedId",
                table: "Subscriptions",
                column: "FeedId",
                principalTable: "Feeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AppUsers_AppUserId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Feeds_FeedId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_FeedId_AppUserId",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_AppUserId",
                table: "Subscription",
                newName: "IX_Subscription_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_FeedId",
                table: "Subscription",
                column: "FeedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_AppUsers_AppUserId",
                table: "Subscription",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Feeds_FeedId",
                table: "Subscription",
                column: "FeedId",
                principalTable: "Feeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
