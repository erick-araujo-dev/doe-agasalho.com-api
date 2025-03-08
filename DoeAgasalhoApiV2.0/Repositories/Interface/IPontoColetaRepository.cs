using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IPontoColetaRepository
    {
        PontoColetaModel GetById(int? id);

        PontoColetaModel GetById(int id);

        PontoColetaModel GetByName(string name);

        List<PontoColetaViewModel> GetByActiveStatus(bool ativo);


        PontoColetaModel Add(PontoColetaCreateModel novoPontoColeta);

        void Update(PontoColetaModel pontoColeta);

        void ActivateCollectPoint(int id);

        void DeactivateCollectPoint(int id);

        bool IsProdutoAssociated(int produtoId, int pontoColetaId);

        Task<List<PontoColetaViewModel>> GetAllActive();
        List<PontoColetaModel> GetAllOrFilteredCollectPoint(int? collectPointId);
    }
}
