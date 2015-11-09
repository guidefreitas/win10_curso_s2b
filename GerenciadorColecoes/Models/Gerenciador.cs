using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorColecoes.Models
{
    public class Gerenciador : IGerenciador
    {
        GerenciadorContext db = new GerenciadorContext();

        public DbSet<Categoria> BuscarCategorias()
        {
            return db.Categorias;
        }

        public DbSet<Livro> BuscarLivros()
        {
            return db.Livros;
        }

        public Categoria BuscarCategoriaPorId(long categoriaId)
        {
            Categoria categoria = db.Categorias.Where(m => m.Id == categoriaId).FirstOrDefault();
            return categoria;
        }

        public Livro BuscarLivroPorId(long livroId)
        {
            Livro livro = db.Livros.Where(m => m.Id == livroId).FirstOrDefault();
            return livro;
        }

        public Categoria AdicionarCategoria(Categoria categoria)
        {
            db.Categorias.Add(categoria);
            db.SaveChanges();
            return categoria;
        }

        public Categoria AtualizarCategoria(Categoria categoria)
        {
            db.Categorias.Update(categoria);
            db.SaveChanges();
            return categoria;
        }

        public void RemoverCategoria(long categoriaId)
        {
            Categoria categoria = db.Categorias.Where(m => m.Id == categoriaId).FirstOrDefault();
            if(categoria != null)
            {
                db.Categorias.Remove(categoria);
                db.SaveChanges();
            }
            
        }

        public Livro AdicionarLivro(Livro livro)
        {
            db.Livros.Add(livro);
            db.SaveChanges();
            return livro;
        }

        public Livro AtualizarLivro(Livro livro)
        {
            db.Livros.Update(livro);
            db.SaveChanges();
            return livro;
        }

        public void RemoverLivro(long livroId)
        {
            Livro livro = db.Livros.Where(m => m.Id == livroId).FirstOrDefault();
            if(livro != null)
            {
                db.Livros.Remove(livro);
                db.SaveChanges();
            }  
        }
 
    }
}
