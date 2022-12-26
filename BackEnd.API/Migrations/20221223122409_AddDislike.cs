using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.API.Migrations
{
    public partial class AddDislike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentDislikes",
                columns: table => new
                {
                    CommentDislikesId = table.Column<int>(type: "int", nullable: false),
                    DislikesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentDislikes", x => new { x.CommentDislikesId, x.DislikesId });
                    table.ForeignKey(
                        name: "FK_CommentDislikes_AspNetUsers_DislikesId",
                        column: x => x.DislikesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentDislikes_Comments_CommentDislikesId",
                        column: x => x.CommentDislikesId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostDislikes",
                columns: table => new
                {
                    DislikesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostDislikesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostDislikes", x => new { x.DislikesId, x.PostDislikesId });
                    table.ForeignKey(
                        name: "FK_PostDislikes_AspNetUsers_DislikesId",
                        column: x => x.DislikesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostDislikes_Posts_PostDislikesId",
                        column: x => x.PostDislikesId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentDislikes_DislikesId",
                table: "CommentDislikes",
                column: "DislikesId");

            migrationBuilder.CreateIndex(
                name: "IX_PostDislikes_PostDislikesId",
                table: "PostDislikes",
                column: "PostDislikesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentDislikes");

            migrationBuilder.DropTable(
                name: "PostDislikes");
        }
    }
}
