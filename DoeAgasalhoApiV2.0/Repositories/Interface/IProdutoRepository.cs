using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IProdutoRepository
    {
        ProdutoModel GetSingleByFilter(Func<ProdutoModel, bool> filter);

        List<ProdutoModel> GetAllByFilter(Func<ProdutoModel, bool> filter);

        void Update(ProdutoModel product);

        void ActivateProduct(int productId);

        void DeactivateProduct(int productId);

        ProdutoModel Add(ProdutoModel product, int collectPointId, int userId);

        List<ProdutoViewModel> GetProdutosByPontoColeta(int pontoColetaId, string ativo = null);

        ProdutoModel GetById(int produtoId);

        IQueryable<ProdutoModel> GetAll();

        List<ProdutoViewModel> GetAllOrFiltered(int? tipoId, int? tamanhoId, string genero, string caracteristica, int collectPointIdAuth);

        List<string> GetCharacteristicsByFilter(int? tipoId, int? tamanhoId, string genero, int collectPointId);

        List<string> GetGenderByFilter(int? tipoId, int? tamanhoId, string? characteristics, int collectPointId);

        List<ProdutoViewModel> GetWithFilter(string tipo, string tamanho, string genero, string caracteristica, int collectPointIdAuth);
    }
}
