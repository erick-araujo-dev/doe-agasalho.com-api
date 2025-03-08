using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class EnderecoModel
{
    public int Id { get; set; }

    public string Logradouro { get; set; } = null!;

    public int Numero { get; set; }

    public string? Complemento { get; set; }

    public string Bairro { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Cep { get; set; } = null!;

    public virtual ICollection<PontoColetaModel> PontoColeta { get; set; } = new List<PontoColetaModel>();
}
