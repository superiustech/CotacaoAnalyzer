using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoCotacao_CotacaoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COTACAO",
                columns: table => new
                {
                    nCdCotacao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sDsCotacao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    tDtCotacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    bFlFreteIncluso = table.Column<bool>(type: "boolean", nullable: false),
                    dVlTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COTACAO", x => x.nCdCotacao);
                });

            migrationBuilder.CreateTable(
                name: "COTACAO_ITEM",
                columns: table => new
                {
                    nCdCotacaoItem = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nSequencial = table.Column<int>(type: "integer", nullable: false),
                    nPrazoEntrega = table.Column<int>(type: "integer", nullable: false),
                    dVlProposto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    nCdProduto = table.Column<int>(type: "integer", nullable: false),
                    nCdCotacao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COTACAO_ITEM", x => x.nCdCotacaoItem);
                    table.ForeignKey(
                        name: "FK_COTACAO_ITEM_COTACAO_nCdCotacao",
                        column: x => x.nCdCotacao,
                        principalTable: "COTACAO",
                        principalColumn: "nCdCotacao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COTACAO_ITEM_PRODUTOS_nCdProduto",
                        column: x => x.nCdProduto,
                        principalTable: "PRODUTOS",
                        principalColumn: "nCdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COTACAO_ITEM_nCdCotacao",
                table: "COTACAO_ITEM",
                column: "nCdCotacao");

            migrationBuilder.CreateIndex(
                name: "IX_COTACAO_ITEM_nCdProduto",
                table: "COTACAO_ITEM",
                column: "nCdProduto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COTACAO_ITEM");

            migrationBuilder.DropTable(
                name: "COTACAO");
        }
    }
}
