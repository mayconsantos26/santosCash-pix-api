using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantosCash.Migrations
{
    /// <inheritdoc />
    public partial class Transacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "keys",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
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
                    id = table.Column<string>(type: "text", nullable: false),
                    txid = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    e2e_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    pagador_nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    pagador_documento = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    pagador_banco = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    pagador_agencia = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    pagador_conta = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    recebedor_nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    recebedor_documento = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    recebedor_banco = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    recebedor_agencia = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    recebedor_conta = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    valor = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Data_Transacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
