using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftsLoggerAPI.Ledana.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "CONVERT(varchar(5), DATEADD(MINUTE, DATEDIFF(minute, [StartTime], [EndTime]), 0),108)", stored: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Amelia", "Aster" },
                    { 2, "Lory", "Marti" },
                    { 3, "Laila", "Martini" },
                    { 4, "Vivian", "Scott" },
                    { 5, "Luiza", "Griffin" },
                    { 6, "Violet", "Jinx" }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "EmployeeId", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 5, 1, 14, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 3, new DateTime(2026, 5, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 4, new DateTime(2026, 5, 1, 14, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 5, new DateTime(2026, 5, 1, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 2, new DateTime(2026, 6, 2, 5, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 23, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 3, new DateTime(2026, 5, 2, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 6, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 4, new DateTime(2026, 5, 2, 14, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 5, new DateTime(2026, 5, 2, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 6, new DateTime(2026, 6, 3, 6, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 22, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, new DateTime(2026, 5, 3, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 3, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 2, new DateTime(2026, 5, 3, 23, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 3, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 5, new DateTime(2026, 5, 4, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 4, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 2, new DateTime(2026, 5, 6, 14, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 6, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 3, new DateTime(2026, 5, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 6, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 4, new DateTime(2026, 5, 7, 14, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 7, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 5, new DateTime(2026, 5, 8, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 2, new DateTime(2026, 6, 9, 5, 35, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 23, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 3, new DateTime(2026, 5, 8, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 6, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 4, new DateTime(2026, 5, 9, 14, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 3, new DateTime(2026, 5, 9, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 6, new DateTime(2026, 6, 10, 6, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 22, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 1, new DateTime(2026, 5, 9, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 2, new DateTime(2026, 5, 9, 23, 25, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 17, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 5, new DateTime(2026, 5, 10, 5, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 21, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
