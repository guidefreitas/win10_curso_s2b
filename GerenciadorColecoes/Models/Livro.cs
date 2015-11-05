using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorColecoes.Models
{
    public class Livro
    {
        public long Id { get; set; }

        public String Nome { get; set; }

        public String Descricao { get; set; }

        public String CaminhoImagem { get; set; }

        public DateTime UltimoAcesso { get; set; }

        public Boolean Favorito { get; set; }

        public virtual Categoria Categoria { get; set; }
    }
}
