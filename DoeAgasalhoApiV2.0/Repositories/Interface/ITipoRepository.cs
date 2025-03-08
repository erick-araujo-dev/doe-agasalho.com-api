using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface ITipoRepository
    {
        TipoModel GetById(int id);

        TipoModel GetByName(string name);

        TipoModel Add(string typeName);

        void Update(TipoModel model);

        List<TipoModel> GetAllTypes();

        List<TipoModel> GetTypesByFilter(int? size, string? gender, string? characteristic, int collectpoint);
    }
}