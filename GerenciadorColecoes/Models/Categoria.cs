using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorColecoes.Models
{
    public class Categoria
    {
        public long Id { get; set; }

        public String Nome { get; set; }

        public virtual ICollection<Livro> Livros { get; set; }
    }
}
