using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SantosCash.Migrations
{
    /// <inheritdoc />
    public partial class TransationsDBHome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "keys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    conta = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keys", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    txid = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    e2e_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    pagador_nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pagador_documento = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    pagador_banco = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    pagador_agencia = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    pagador_conta = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    recebedor_nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    recebedor_documento = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    recebedor_banco = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    recebedor_agencia = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    recebedor_conta = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    valor = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    data_transacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transacoes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "keys");

            migrationBuilder.DropTable(
                name: "transacoes");
        }
    }
}
