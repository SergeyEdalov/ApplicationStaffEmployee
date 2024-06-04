using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppStaffEmployee.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    birth_day = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    department = table.Column<string>(type: "text", nullable: false),
                    job_title = table.Column<string>(type: "text", nullable: false),
                    date_of_start_work = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    salary = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("employee_pkey", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "id", "birth_day", "department", "full_name", "job_title", "salary", "date_of_start_work" },
                values: new object[,]
                {
                    { new Guid("67bbefef-a586-4416-b5f5-8d70f3b51d44"), new DateTime(1991, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "bookkeeping", "Bastriykina Maria Sergeevna", "accountant", 60000.0m, new DateTime(2015, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6afd4cd6-0fe0-4d0b-aa72-bd4bf97a4860"), new DateTime(1978, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "bookkeeping", "Konovalova Irina Alekseevna", "chief accountant", 90000.0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a0cf8d7c-f8a1-460a-9b97-14a58fae574f"), new DateTime(1990, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "financial", "Ermakov Sergey Vasilevich", "financial director", 100000.0m, new DateTime(2022, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("af742a65-2b76-4895-91cf-7e35019309fd"), new DateTime(1994, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "sale", "Csvetov Victor Andreevich", "chief Sales Specialist", 115000.0m, new DateTime(2017, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("bc3d5e78-fe17-4e95-b08e-a56d58384325"), new DateTime(1985, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "marketing", "Popov Ivan Nikolaevich", "trainee", 56000.0m, new DateTime(2023, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48bbf"), new DateTime(1996, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "development", "Djekson Bob", "middle devops", 80000.0m, new DateTime(2020, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employees");
        }
    }
}
