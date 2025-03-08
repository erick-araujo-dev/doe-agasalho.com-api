using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class PontoProdutoModel
{
    public int PontoColetaId { get; set; }

    public int ProdutoId { get; set; }

    public virtual ProdutoModel Produto { get; set; } = null!;
}
