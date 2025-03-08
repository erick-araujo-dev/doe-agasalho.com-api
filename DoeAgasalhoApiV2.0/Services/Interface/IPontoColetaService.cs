using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services
{
    public interface IPontoColetaService
    {
        List<PontoColetaViewModel> GetActivateCollectPoint();

        List<PontoColetaViewModel> GetInactiveCollectPoint();

        PontoColetaModel CreateCollectPoint(PontoColetaCreateModel novoPontoColeta);

        PontoColetaModel UpdateCollectPoint(int id, PontoColetaCreateModel pontoColeta);

        void ActivateCollectPoint(int id);

        void DeactivateCollectPoint(int id);

        PontoColetaModel GetById(int id);

        Task<List<PontoColetaViewModel>> GetAllActive();
        List<PontoColetaModel> GetAllOrFilteredCollectPoint(int? collectPointId);
    }
}
