using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorColecoes.Models
{
    public interface IGerenciador
    {
        DbSet<Categoria> BuscarCategorias();
        DbSet<Livro> BuscarLivros();
        Categoria BuscarCategoriaPorId(long categoriaId);
        Livro BuscarLivroPorId(long livroId);

        Categoria AdicionarCategoria(Categoria categoria);
        Categoria AtualizarCategoria(Categoria categoria);
        void RemoverCategoria(long categoriaId);
        
        Livro AdicionarLivro(Livro livro);
        Livro AtualizarLivro(Livro livro);
        void RemoverLivro(long livroId);
        

    }
}
