using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace GerenciadorColecoes.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CaminhoImagem = table.Column<string>(nullable: true),
                    CategoriaId = table.Column<long>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    Favorito = table.Column<bool>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    UltimoAcesso = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livro_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Livro");
            migrationBuilder.DropTable("Categoria");
        }
    }
}
