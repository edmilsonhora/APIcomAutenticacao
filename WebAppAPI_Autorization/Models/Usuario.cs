using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppAPI_Autorization.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Role { get; set; }

        public Usuario ObterPor(string nome, string senha)
        {
            var lista = new List<Usuario>();
            lista.Add(new Usuario(){Id=1, Nome = "joao",Senha = "123456",Role = "User"});
            lista.Add(new Usuario() { Id = 2, Nome = "maria", Senha = "123456", Role = "User" });
            lista.Add(new Usuario() { Id = 3, Nome = "pedro", Senha = "123456", Role = "User" });

            return lista.FirstOrDefault(p => p.Nome == nome && p.Senha == senha);

        }
    }
}