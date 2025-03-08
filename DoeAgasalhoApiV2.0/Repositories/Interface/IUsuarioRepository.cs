using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repository.Interface
{
    public interface IUsuarioRepository

    {
        List<UsuarioModel> GetAll();

        UsuarioModel GetByEmail(string email);

/*        List<UsuarioModel> GetByActiveStatus(bool ativo);
*/
        UsuarioModel GetById(int id);

        UsuarioModel GetByUserName(string name);

        UsuarioModel GetByEmailAndPassword(string email, string password);

        UsuarioModel Add(UsuarioCreateModel usuario);

        void Update(UsuarioModel usuario);

        void ActivateUser(int userId);

        void DeactivateUser(int userId);

        IEnumerable<UsuarioModel> GetUsuariosByPontoColetaId(int? pontoColetaId);

        List<UsuarioViewModel> GetByActiveStatus(bool ativo);
    }
}
