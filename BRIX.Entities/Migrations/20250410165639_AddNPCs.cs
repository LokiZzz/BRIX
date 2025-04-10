using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BRIX.GameService.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddNPCs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NPC",
                schema: "chr",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NPCJsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NPC_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "acc",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NPC_UserId",
                schema: "chr",
                table: "NPC",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NPC",
                schema: "chr");
        }
    }
}
