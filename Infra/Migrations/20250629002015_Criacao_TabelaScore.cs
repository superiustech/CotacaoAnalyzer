using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Criacao_TabelaScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COTACAO_SCORE",
                columns: table => new
                {
                    nCdScore = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nPesoValor = table.Column<int>(type: "integer", nullable: false),
                    nPesoFreteIncluso = table.Column<int>(type: "integer", nullable: false),
                    nPesoPrazoEntrega = table.Column<int>(type: "integer", nullable: false),
                    tDtCriacaoPeso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COTACAO_SCORE", x => x.nCdScore);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COTACAO_SCORE");
        }
    }
}
