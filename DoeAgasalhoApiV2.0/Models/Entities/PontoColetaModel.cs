using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class PontoColetaModel
{
    public int Id { get; set; }

    public string NomePonto { get; set; } = null!;

    public string? Ativo { get; set; }

    public int EnderecoId { get; set; }

    public virtual EnderecoModel Endereco { get; set; } = null!;

}
