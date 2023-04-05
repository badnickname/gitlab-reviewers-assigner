using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitlabMonitor.Migrations
{
    /// <inheritdoc />
    public partial class index_on_reviewer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviewers_UserId",
                table: "Reviewers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviewers_UserId",
                table: "Reviewers");
        }
    }
}
