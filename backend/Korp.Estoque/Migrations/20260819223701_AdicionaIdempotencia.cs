using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Korp.Estoque.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaIdempotencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperacoesIdempotencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chave = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Operacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CriadaEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacoesIdempotencia", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperacoesIdempotencia_Chave_Operacao",
                table: "OperacoesIdempotencia",
                columns: new[] { "Chave", "Operacao" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperacoesIdempotencia");
        }
    }
}
