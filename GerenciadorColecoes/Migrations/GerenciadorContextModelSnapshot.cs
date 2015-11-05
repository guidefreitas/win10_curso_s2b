using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using GerenciadorColecoes.Models;

namespace GerenciadorColecoes.Migrations
{
    [DbContext(typeof(GerenciadorContext))]
    partial class GerenciadorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964");

            modelBuilder.Entity("GerenciadorColecoes.Models.Categoria", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("GerenciadorColecoes.Models.Livro", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CaminhoImagem");

                    b.Property<long?>("CategoriaId");

                    b.Property<string>("Descricao");

                    b.Property<bool>("Favorito");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<DateTime>("UltimoAcesso");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("GerenciadorColecoes.Models.Livro", b =>
                {
                    b.HasOne("GerenciadorColecoes.Models.Categoria")
                        .WithMany()
                        .ForeignKey("CategoriaId");
                });
        }
    }
}
