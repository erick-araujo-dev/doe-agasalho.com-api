using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface ILoginService
    {
        UsuarioModel Authenticate(LoginModel model);
    }
}
