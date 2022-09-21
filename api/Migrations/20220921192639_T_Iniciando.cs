using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Pesagem.Migrations
{
    public partial class T_Iniciando : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fazendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Inativo = table.Column<short>(type: "smallint", nullable: false),
                    IdAlphaExpress = table.Column<int>(type: "integer", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fazendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAlphaExpress = table.Column<int>(type: "integer", nullable: true),
                    NomeRazao = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Apelido = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    TipoPessoa = table.Column<short>(type: "smallint", nullable: false),
                    Cpf = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Rg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Cep = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    Endereco = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Bairro = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    Numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: true),
                    Celular = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: true),
                    Contato = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Observacao = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true),
                    Inativo = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: true),
                    Bloqueado = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: true),
                    Vendedores = table.Column<string>(type: "text", nullable: true),
                    CodigoIbgeCidade = table.Column<int>(type: "integer", nullable: true),
                    Sincronizado = table.Column<short>(type: "smallint", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Aviso = table.Column<string>(type: "text", nullable: true),
                    FazendaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fornecedor_Fazendas_FazendaId",
                        column: x => x.FazendaId,
                        principalTable: "Fazendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAlphaExpress = table.Column<int>(type: "integer", nullable: true),
                    Mensagem = table.Column<string>(type: "text", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FazendaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Fazendas_FazendaId",
                        column: x => x.FazendaId,
                        principalTable: "Fazendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Peso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    FazendaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peso_Fazendas_FazendaId",
                        column: x => x.FazendaId,
                        principalTable: "Fazendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fazendas_Id",
                table: "Fazendas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_FazendaId",
                table: "Fornecedor",
                column: "FazendaId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_FazendaId",
                table: "Log",
                column: "FazendaId");

            migrationBuilder.CreateIndex(
                name: "IX_Peso_FazendaId",
                table: "Peso",
                column: "FazendaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Peso");

            migrationBuilder.DropTable(
                name: "Fazendas");
        }
    }
}
