using System.Text.Json.Serialization;

namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class ChangeCollectPointModel
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public int? PontoColetaId { get; set; }

        [JsonIgnore]
        public string? Tipo { get; set; }

    }
}
