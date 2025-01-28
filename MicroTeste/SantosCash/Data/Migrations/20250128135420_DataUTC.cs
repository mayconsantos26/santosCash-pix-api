using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantosCash.Migrations
{
    /// <inheritdoc />
    public partial class DataUTC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_transacao",
                table: "transacoes",
                type: "timestamp(3) with time zone",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_transacao",
                table: "transacoes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldPrecision: 3);
        }
    }
}
