using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class DoacaoViewModel
    {
        public int Id { get; set; }

        public string TipoMovimento { get; set; }

        public int Quantidade { get; set; }

        public string Tipo { get; set; }

        public string Tamanho { get; set; }

        public string PontoColeta { get; set; }

        public string Caracteristica { get; set; }

        public string Usuario { get; set; }

        public DateTime DataMovimento { get; set; }

    }
}
