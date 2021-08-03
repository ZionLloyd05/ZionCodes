using Microsoft.EntityFrameworkCore.Migrations;

namespace ZionCodes.Core.Migrations
{
    public partial class RemovedArticleCommentAndRemodelComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<byte[]>(
                name: "ArticleId",
                table: "Comments",
                type: "varbinary(16)",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Comments");

            migrationBuilder.CreateTable(
                name: "ArticleComments",
                columns: table => new
                {
                    ArticleId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    CommentId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComments", x => new { x.ArticleId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_ArticleComments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArticleComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_CommentId",
                table: "ArticleComments",
                column: "CommentId");
        }
    }
}
