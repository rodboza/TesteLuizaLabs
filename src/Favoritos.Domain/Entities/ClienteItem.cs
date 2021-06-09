using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Favoritos.Domain.Entities
{
    public class ClienteItem : Entity
    {
        [Required]
        public String Nome { get; set; }
        [Required]
        public String Email { get; set; }
        //public IEnumerable<FavoritoItem> Favoritos { get; set; }
        public List<FavoritoItem> ListaProdutosFavoritos { get; set; }



        public ClienteItem(String nome, String email)
        {
            Nome = nome;
            Email = email;
            ListaProdutosFavoritos = new List<FavoritoItem>();
        }
    }
}
