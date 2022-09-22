using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Pesagem.Migrations
{
    public partial class T_InitialCreate : Migration
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
                    Aviso = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAlphaExpress = table.Column<int>(type: "integer", nullable: true),
                    Mensagem = table.Column<string>(type: "text", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Brinco = table.Column<string>(type: "text", nullable: true),
                    BrincoEletronico = table.Column<string>(type: "text", nullable: true),
                    PesoTotal = table.Column<string>(type: "text", nullable: true),
                    FazendaId = table.Column<string>(type: "text", nullable: true),
                    Idade = table.Column<string>(type: "text", nullable: true),
                    Raca = table.Column<string>(type: "text", nullable: true),
                    ValorMedio = table.Column<string>(type: "text", nullable: true),
                    Sexo = table.Column<string>(type: "text", nullable: true),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    Sincronizado = table.Column<short>(type: "smallint", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdAlphaExpress = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peso", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fazendas");

            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Peso");
        }
    }
}
