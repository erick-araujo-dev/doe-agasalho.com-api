using System.Text.Json.Serialization;

namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class DoacaoEntradaSaidaModel
    {
        [JsonIgnore] 
        public int Id { get; set; } 

        public int Quantidade { get; set; }

        public int ProdutoId { get; set; }
    }
}
