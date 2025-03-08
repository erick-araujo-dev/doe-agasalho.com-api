using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IDoacaoService
    {
        Task<DoacaoModel> DoacaoProduto(DoacaoEntradaSaidaModel model, string tipoMovimento);

        DoacaoViewModel ExibirDoacaoPorId(int id);
        List<DoacaoViewModel> FiltrarDoacoes(int mes, int usuarioId, string tipoMovimento);
        List<DoacaoViewModel> GetAllDonations(int? collectPointId, int? userId, int? day, int? month, int? year, string? typeOfMovement);
        Task<DoacaoModel> GetDoacao(int id);
    }
}