using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IDoacaoRepository
    {
        Task<DoacaoModel> Add(int produtoId, int quantidade, int userId, string tipoMovimento);


        Task<DoacaoModel> GetDoacao(int id);

        DoacaoModel GetById(int id);

        IQueryable<DoacaoModel> GetAll();

/*        List<DoacaoViewModel> GetAllDonations(int? collectPointId, int? userId, int? day, int? month, int? year, string? typeOfMovement);
*/    }
}