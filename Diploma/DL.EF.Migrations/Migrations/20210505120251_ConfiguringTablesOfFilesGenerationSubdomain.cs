using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DL.EF.Migrations.Migrations
{
    public partial class ConfiguringTablesOfFilesGenerationSubdomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneratedFiles_FileTypes_FileTypeId",
                table: "GeneratedFiles");

            migrationBuilder.DropTable(
                name: "FileTypes");

            migrationBuilder.DropIndex(
                name: "IX_GeneratedFiles_FileTypeId",
                table: "GeneratedFiles");

            migrationBuilder.DropColumn(
                name: "FileTypeId",
                table: "GeneratedFiles");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "GeneratedFiles");

            migrationBuilder.AddColumn<int>(
                name: "Format",
                table: "GeneratedFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "GeneratedFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "GeneratedFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedFiles_UserId",
                table: "GeneratedFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneratedFiles_Users_UserId",
                table: "GeneratedFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneratedFiles_Users_UserId",
                table: "GeneratedFiles");

            migrationBuilder.DropIndex(
                name: "IX_GeneratedFiles_UserId",
                table: "GeneratedFiles");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "GeneratedFiles");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "GeneratedFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GeneratedFiles");

            migrationBuilder.AddColumn<long>(
                name: "FileTypeId",
                table: "GeneratedFiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "GeneratedFiles",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FileTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1L, 0 },
                    { 2L, 1 },
                    { 3L, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedFiles_FileTypeId",
                table: "GeneratedFiles",
                column: "FileTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneratedFiles_FileTypes_FileTypeId",
                table: "GeneratedFiles",
                column: "FileTypeId",
                principalTable: "FileTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
