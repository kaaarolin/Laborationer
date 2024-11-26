using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb_2_databaser.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Butiker",
                columns: table => new
                {
                    ButikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Butiksnamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postnummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Butiker", x => x.ButikId);
                });

            migrationBuilder.CreateTable(
                name: "Böcker",
                columns: table => new
                {
                    ISBN13 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Språk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Utgivningsdatum = table.Column<DateOnly>(type: "date", nullable: true),
                    FörfattareId = table.Column<int>(type: "int", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntalSidor = table.Column<int>(type: "int", nullable: true),
                    Förlag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Böcker", x => x.ISBN13);
                });

            migrationBuilder.CreateTable(
                name: "Kunder",
                columns: table => new
                {
                    KundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Förnamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Efternamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Epost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunder", x => x.KundId);
                });

            migrationBuilder.CreateTable(
                name: "Lagersaldo",
                columns: table => new
                {
                    ButikId = table.Column<int>(type: "int", nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lagersaldo", x => x.ButikId);
                    table.ForeignKey(
                        name: "FK_Lagersaldo_Butiker_ButikId",
                        column: x => x.ButikId,
                        principalTable: "Butiker",
                        principalColumn: "ButikId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lagersaldo_Böcker_Isbn",
                        column: x => x.Isbn,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ordrar",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundId = table.Column<int>(type: "int", nullable: true),
                    OrderDatum = table.Column<DateOnly>(type: "date", nullable: true),
                    TotaltPris = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordrar", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Ordrar_Kunder_KundId",
                        column: x => x.KundId,
                        principalTable: "Kunder",
                        principalColumn: "KundId");
                });

            migrationBuilder.CreateTable(
                name: "Recensioner",
                columns: table => new
                {
                    RecensionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Boktitel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KundId = table.Column<int>(type: "int", nullable: true),
                    Betyg = table.Column<int>(type: "int", nullable: true),
                    RecensionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recensionsdatum = table.Column<DateOnly>(type: "date", nullable: true),
                    IsbnNavigationISBN13 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recensioner", x => x.RecensionId);
                    table.ForeignKey(
                        name: "FK_Recensioner_Böcker_IsbnNavigationISBN13",
                        column: x => x.IsbnNavigationISBN13,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13");
                    table.ForeignKey(
                        name: "FK_Recensioner_Kunder_KundId",
                        column: x => x.KundId,
                        principalTable: "Kunder",
                        principalColumn: "KundId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lagersaldo_Isbn",
                table: "Lagersaldo",
                column: "Isbn");

            migrationBuilder.CreateIndex(
                name: "IX_Ordrar_KundId",
                table: "Ordrar",
                column: "KundId");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioner_IsbnNavigationISBN13",
                table: "Recensioner",
                column: "IsbnNavigationISBN13");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioner_KundId",
                table: "Recensioner",
                column: "KundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lagersaldo");

            migrationBuilder.DropTable(
                name: "Ordrar");

            migrationBuilder.DropTable(
                name: "Recensioner");

            migrationBuilder.DropTable(
                name: "Butiker");

            migrationBuilder.DropTable(
                name: "Böcker");

            migrationBuilder.DropTable(
                name: "Kunder");
        }
    }
}
