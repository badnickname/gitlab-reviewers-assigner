﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitlabMonitor.Migrations
{
    /// <inheritdoc />
    public partial class projectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MergeId",
                table: "AssignedMergeRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "AssignedMergeRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MergeId",
                table: "AssignedMergeRequests");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AssignedMergeRequests");
        }
    }
}
