using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class TaskAssignmentAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Tasks_TaskId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Users_UserId",
                table: "TaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TaskAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AssignedAt",
                table: "TaskAssignments",
                type: "datetimeoffset(3)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedByUserId",
                table: "TaskAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "TaskAssignments",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_AssignedByUserId",
                table: "TaskAssignments",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_TaskId_UserId",
                table: "TaskAssignments",
                columns: new[] { "TaskId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Tasks_TaskId",
                table: "TaskAssignments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Users_AssignedByUserId",
                table: "TaskAssignments",
                column: "AssignedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Users_UserId",
                table: "TaskAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Tasks_TaskId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Users_AssignedByUserId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Users_UserId",
                table: "TaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignments_AssignedByUserId",
                table: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignments_TaskId_UserId",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "TaskAssignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments",
                columns: new[] { "TaskId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Tasks_TaskId",
                table: "TaskAssignments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Users_UserId",
                table: "TaskAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
