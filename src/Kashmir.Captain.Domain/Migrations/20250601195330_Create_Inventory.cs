using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kashmir.Captain.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Create_Inventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.CreateTable(
                name: "Machines",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreateDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<int>(type: "int", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifySource = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreateDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<int>(type: "int", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifySource = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreateDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<int>(type: "int", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifySource = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Machines",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Tools",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Workers",
                schema: "inventory");
        }
    }
}
