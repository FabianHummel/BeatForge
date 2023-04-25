using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeatForgeClient.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "p_preferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Volume = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_p_preferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "s_song",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PreferencesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_song", x => x.Id);
                    table.ForeignKey(
                        name: "FK_s_song_p_preferences_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "p_preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "c_channel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SongId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_channel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_c_channel_s_song_SongId",
                        column: x => x.SongId,
                        principalTable: "s_song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "i_instrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_i_instrument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_i_instrument_c_channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "c_channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "n_note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Start = table.Column<decimal>(type: "TEXT", nullable: false),
                    Duration = table.Column<decimal>(type: "TEXT", nullable: false),
                    Pitch = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_n_note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_n_note_c_channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "c_channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_c_channel_SongId",
                table: "c_channel",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_i_instrument_ChannelId",
                table: "i_instrument",
                column: "ChannelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_n_note_ChannelId",
                table: "n_note",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_s_song_PreferencesId",
                table: "s_song",
                column: "PreferencesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "i_instrument");

            migrationBuilder.DropTable(
                name: "n_note");

            migrationBuilder.DropTable(
                name: "c_channel");

            migrationBuilder.DropTable(
                name: "s_song");

            migrationBuilder.DropTable(
                name: "p_preferences");
        }
    }
}
