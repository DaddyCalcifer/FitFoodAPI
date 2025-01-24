using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFoodAPI.Migrations
{
    /// <inheritdoc />
    public partial class feedacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedActs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    FeedType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Mass = table.Column<double>(type: "double precision", nullable: false),
                    Kcal100 = table.Column<double>(type: "double precision", nullable: false),
                    Fat100 = table.Column<double>(type: "double precision", nullable: false),
                    Protein100 = table.Column<double>(type: "double precision", nullable: false),
                    Carb100 = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedActs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedActs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedActs_UserId",
                table: "FeedActs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedActs");
        }
    }
}
