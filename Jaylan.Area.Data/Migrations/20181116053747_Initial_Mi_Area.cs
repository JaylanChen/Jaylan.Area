using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jaylan.Area.Data.Migrations
{
    public partial class Initial_Mi_Area : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mi_Area",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentCode = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    ZipCode = table.Column<string>(unicode: false, maxLength: 16, nullable: true),
                    Level = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    MiId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mi_Area", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mi_Area");
        }
    }
}
