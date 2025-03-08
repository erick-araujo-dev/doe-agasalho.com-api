namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class UsuarioCreateModel
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Tipo { get; set; }

        public int? PontoColetaId { get; set; }
    }
}
