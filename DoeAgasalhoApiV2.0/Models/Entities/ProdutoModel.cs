using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class ProdutoModel
{
    public int Id { get; set; }

    public string Ativo { get; set; } = null!;

    public string Caracteristica { get; set; } = null!;

    public int TamanhoId { get; set; }

    public int TipoId { get; set; }

    public string Genero { get; set; } = null!;

    public int Estoque { get; set; }

    public virtual ICollection<DoacaoModel> Doacoes { get; set; } = new List<DoacaoModel>();

    public virtual ICollection<PontoProdutoModel> PontoProdutos { get; set; } = new List<PontoProdutoModel>();

    public virtual TamanhoModel Tamanho { get; set; } = null!;

    public virtual TipoModel Tipo { get; set; } = null!;
}
