using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface ITamanhoService
    {
        TamanhoModel CreateNewSize(string size);

        TamanhoModel GetById(int id);

        List<TamanhoModel> GetSizesByFilter(int? type, string? gender, string? characteristics);
    }
}