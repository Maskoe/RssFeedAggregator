using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockStuff.Migrations
{
    /// <inheritdoc />
    public partial class Added_ApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""ALTER TABLE "AppUsers" ADD COLUMN "ApiKey" TEXT UNIQUE NOT NULL DEFAULT (encode(sha256(random()::text::bytea), 'hex'))""");
            
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "AppUsers");
        }
    }
}
