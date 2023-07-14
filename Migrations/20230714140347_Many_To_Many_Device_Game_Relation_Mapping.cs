using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestfulGamesApi.Migrations
{
    /// <inheritdoc />
    public partial class Many_To_Many_Device_Game_Relation_Mapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Games_GameId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_GameId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Devices");

            migrationBuilder.CreateTable(
                name: "DeviceGame",
                columns: table => new
                {
                    AvailableDevicesId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceGame", x => new { x.AvailableDevicesId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_DeviceGame_Devices_AvailableDevicesId",
                        column: x => x.AvailableDevicesId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceGame_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceGame_GamesId",
                table: "DeviceGame",
                column: "GamesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceGame");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Devices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_GameId",
                table: "Devices",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Games_GameId",
                table: "Devices",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
