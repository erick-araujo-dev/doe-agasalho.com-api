using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface ITipoService
    {
        TipoModel CreateNewType(string type);

        List<TipoModel> GetAllTypes();

        TipoModel GetById(int id);

        List<TipoModel> GetTypesByFilter(int? size, string? gender, string? characteristics);
    }
}