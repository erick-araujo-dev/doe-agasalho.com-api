using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface ITokenService
    {
        public string GenerateToken(UsuarioModel usuario);
    }
}
