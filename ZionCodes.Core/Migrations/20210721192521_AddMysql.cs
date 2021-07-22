using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZionCodes.Core.Migrations
{
    public partial class AddMysql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    CreatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    UpdatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    Upvote = table.Column<int>(type: "int", nullable: false),
                    Downvote = table.Column<int>(type: "int", nullable: false),
                    ParentCommentId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadingNotes",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: false),
                    Heart = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    CreatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    UpdatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    CreatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    UpdatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: false),
                    Heart = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp", nullable: false),
                    CreatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    UpdatedBy = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ReadingNotes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
