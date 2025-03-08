using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class TipoModel
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string Ativo { get; set; }

    public virtual ICollection<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
}
