using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IProdutoService
    {
        void ActivateProduct(int id);

        void DeactivateProduct(int id);

        ProdutoModel CreateProduct(ProdutoCreateModel newProductCreate);

        List<ProdutoViewModel> GetAllProdutos();

        List<ProdutoViewModel> GetByActiveProdutos();

        List<ProdutoViewModel> GetByInactiveProdutos();

        ProdutoViewModel GetProdutoById(int id);

        ProdutoModel UpdateProduct(int id, ProdutoCreateModel product);

        List<ProdutoViewModel> GetAllOrFiltered(int? tipoId, int? tamanhoId, string? genero, string? caracteristica);

        List<string> GetCharacteristicsByFilter(int? tipoId, int? tamanhoId, string genero);

        List<string> GetGenderByFilters(int? tipoId, int? tamanhoId, string? characteristics);

        List<ProdutoViewModel> GetProductsWithFilter(string type, string size, string gender, string characteristic);
    }
}