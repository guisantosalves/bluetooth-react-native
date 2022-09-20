using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Pesagem.Migrations
{
    public partial class T_Ajustando_Peso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Peso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FazendaId = table.Column<int>(type: "integer", nullable: false),
                    Brinco = table.Column<string>(type: "text", nullable: true),
                    BrincoEletronico = table.Column<string>(type: "text", nullable: true),
                    PesoTotal = table.Column<string>(type: "text", nullable: true),
                    Idade = table.Column<string>(type: "text", nullable: true),
                    Raca = table.Column<string>(type: "text", nullable: true),
                    ValorMedio = table.Column<string>(type: "text", nullable: true),
                    Sexo = table.Column<string>(type: "text", nullable: true),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    Sincronizado = table.Column<short>(type: "smallint", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdAlphaExpress = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peso_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Peso_EmpresaId",
                table: "Peso",
                column: "EmpresaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Peso");
        }
    }
}
