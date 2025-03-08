using System.Text.Json.Serialization;

namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class PontoColetaCreateModel
    {
        [JsonIgnore] 
        public int Id { get; set; }

        public string NomePonto { get; set; }

        public string Logradouro { get; set; }

        public int Numero { get; set; }

        public string? Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Cep { get; set; }
    }
}
