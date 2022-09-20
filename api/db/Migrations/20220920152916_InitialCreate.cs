using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Pesagem.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    ImagemBase64 = table.Column<string>(type: "text", nullable: true),
                    Inativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Senha = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Salto = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    IdAlphaExpress = table.Column<int>(type: "integer", nullable: true),
                    Inativo = table.Column<short>(type: "smallint", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    UsuarioCriacaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Aviso = table.Column<string>(type: "text", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fornecedor_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fornecedor_Usuarios_UsuarioAlteracaoId",
                        column: x => x.UsuarioAlteracaoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fornecedor_Usuarios_UsuarioCriacaoId",
                        column: x => x.UsuarioCriacaoId,
                        principalTable: "Usuarios",
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
                    UsuarioCriacaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_Usuarios_UsuarioAlteracaoId",
                        column: x => x.UsuarioAlteracaoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_Usuarios_UsuarioCriacaoId",
                        column: x => x.UsuarioCriacaoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_EmpresaId",
                table: "Fornecedor",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_UsuarioAlteracaoId",
                table: "Fornecedor",
                column: "UsuarioAlteracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_UsuarioCriacaoId",
                table: "Fornecedor",
                column: "UsuarioCriacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_EmpresaId",
                table: "Log",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UsuarioAlteracaoId",
                table: "Log",
                column: "UsuarioAlteracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UsuarioCriacaoId",
                table: "Log",
                column: "UsuarioCriacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpresaId",
                table: "Usuarios",
                column: "EmpresaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
