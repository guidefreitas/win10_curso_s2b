using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorColecoes.Models
{
    public class Gerenciador
    {
        GerenciadorContext db = new GerenciadorContext();

        public Categoria AdicionarCategoria(Categoria categoria)
        {
            db.Categorias.Add(categoria);
            db.SaveChanges();
            return categoria;
        }

        public void RemoverCategoria(long categoriaId)
        {
            Categoria categoria = db.Categorias.Where(m => m.Id == categoriaId).FirstOrDefault();
            db.Categorias.Remove(categoria);
            db.SaveChanges();
        }


    }
}
