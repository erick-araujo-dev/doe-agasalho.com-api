using System.Text.Json.Serialization;

namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; } 

        public string Tipo { get; set; } 

        public string Ativo { get; set; } 

        public string PontoColeta { get; set; }
    }
}
