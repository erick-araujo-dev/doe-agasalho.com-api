using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IUsuarioService

    {

        bool IsActiveUser(UsuarioModel user);

        UsuarioModel CreateUser(UsuarioCreateModel usuario);

        UsuarioModel UpdateUsername(int id, UpdateUsernameModel user);

        UsuarioModel ChangeCollectPoint(int id, ChangeCollectPointModel usuario);

        UsuarioModel ChangePassword(int id, ChangePasswordModel user);

        void ActivateUser(int id);

        void DeactivateUser(int id);

        List<UsuarioModel> GetAllUsers();

        List<UsuarioViewModel> GetActiveUsers();

        List<UsuarioViewModel> GetInactiveUsers();

        UsuarioModel GetById(int id);

        IEnumerable<UsuarioModel> GetUserByCollectPoint(int? collectPoint);
    }
}
