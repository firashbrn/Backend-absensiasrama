using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Gedungs",
                columns: table => new
                {
                    IdGedung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NamaGedung = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gedungs", x => x.IdGedung);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kamars",
                columns: table => new
                {
                    NoKamar = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Lantai = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdGedung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kamars", x => x.NoKamar);
                    table.ForeignKey(
                        name: "FK_Kamars_Gedungs_IdGedung",
                        column: x => x.IdGedung,
                        principalTable: "Gedungs",
                        principalColumn: "IdGedung",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nama = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nim = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Asal = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JenisKelamin = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    NoKamar = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.CheckConstraint("CK_User_JenisKelamin", "JenisKelamin IN ('L', 'P')");
                    table.ForeignKey(
                        name: "FK_Users_Kamars_NoKamar",
                        column: x => x.NoKamar,
                        principalTable: "Kamars",
                        principalColumn: "NoKamar",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Perizinans",
                columns: table => new
                {
                    IdIzin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Alasan = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileBukti = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DiprosesOlehIdUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perizinans", x => x.IdIzin);
                    table.ForeignKey(
                        name: "FK_Perizinans_Users_DiprosesOlehIdUser",
                        column: x => x.DiprosesOlehIdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Perizinans_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Presensis",
                columns: table => new
                {
                    IdPresensi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Waktu = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Sesi = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FotoWajah = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Keterangan = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presensis", x => x.IdPresensi);
                    table.ForeignKey(
                        name: "FK_Presensis_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Gedungs_IdUser",
                table: "Gedungs",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Kamars_IdGedung",
                table: "Kamars",
                column: "IdGedung");

            migrationBuilder.CreateIndex(
                name: "IX_Perizinans_DiprosesOlehIdUser",
                table: "Perizinans",
                column: "DiprosesOlehIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Perizinans_IdUser",
                table: "Perizinans",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Presensis_IdUser_Tanggal_Sesi",
                table: "Presensis",
                columns: new[] { "IdUser", "Tanggal", "Sesi" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Presensis_Tanggal",
                table: "Presensis",
                column: "Tanggal");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nim",
                table: "Users",
                column: "Nim",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NoKamar",
                table: "Users",
                column: "NoKamar");

            migrationBuilder.AddForeignKey(
                name: "FK_Gedungs_Users_IdUser",
                table: "Gedungs",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gedungs_Users_IdUser",
                table: "Gedungs");

            migrationBuilder.DropTable(
                name: "Perizinans");

            migrationBuilder.DropTable(
                name: "Presensis");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Kamars");

            migrationBuilder.DropTable(
                name: "Gedungs");
        }
    }
}
