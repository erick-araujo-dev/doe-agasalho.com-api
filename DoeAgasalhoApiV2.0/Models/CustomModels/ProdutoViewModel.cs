using DoeAgasalhoApiV2._0.Models.Entities;
using System.Text.Json.Serialization;

namespace DoeAgasalhoApiV2._0.Models.Custom_Models
{
    public class ProdutoViewModel
    {

        public int Id { get; set; }
        
        public string Ativo { get; set; }     

        public string Tipo { get; set; }
        
        public string Caracteristica { get; set; }

        public string Tamanho { get; set; }

        public string Genero { get; set; }

        public int Estoque { get; set; }


    }
}
