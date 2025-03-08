using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class UsuarioModel
{
    public int Id { get; set; }

    public string Nome { get; set; } 

    public string Email { get; set; } 

    public string Senha { get; set; } 

    public string Tipo { get; set; } 

    public string Ativo { get; set; } 

    public int? PontoColetaId { get; set; }

    public virtual ICollection<DoacaoModel> Doacoes { get; set; } = new List<DoacaoModel>();
}
