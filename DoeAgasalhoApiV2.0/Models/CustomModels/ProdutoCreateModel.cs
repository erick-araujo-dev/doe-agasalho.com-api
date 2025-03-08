using System.Text.Json.Serialization;

namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class ProdutoCreateModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Caracteristica { get; set; }

        public string Tamanho { get; set; }

        public string Tipo { get; set; }

        public string Genero { get; set; }

    }
}