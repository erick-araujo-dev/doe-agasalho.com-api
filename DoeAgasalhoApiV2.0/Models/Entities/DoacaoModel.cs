using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class DoacaoModel
{
    public int Id { get; set; }

    public string TipoMovimento { get; set; }

    public DateTime DataMovimento { get; set; }

    public int Quantidade { get; set; }

    public int ProdutoId { get; set; }

    public int UsuarioId { get; set; }

    public virtual ProdutoModel Produto { get; set; }

    public virtual UsuarioModel Usuario { get; set; }
}
