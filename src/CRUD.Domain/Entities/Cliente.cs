using System;

namespace CRUD.Domain.Entities
{
     public class Cliente
    {
        //Determina Chave Primaria
        //[Key]
        public int Id { get; set; }
        //Determina Campo Obrigatorio
        //[Required]
        public string Nome { get; set; }
        //Determina Nome de coluna quando quiser q seja diferente
        //[Column("Phone")]
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Email {get; set;}

    }
}
