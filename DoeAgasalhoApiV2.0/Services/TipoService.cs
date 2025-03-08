using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TipoService : ITipoService
    {
        private readonly ITipoRepository _tipoRepository;
        private readonly IUtilsService _utilsService;

        public TipoService (ITipoRepository tipoRepository, IUtilsService utilsService)
        {
            _tipoRepository = tipoRepository;
            _utilsService = utilsService;   
        }
        public TipoModel CreateNewType(string type)
        {
            var newType = _tipoRepository.Add(type);
            return newType;
        }

        public List<TipoModel> GetAllTypes()
        {
            return _tipoRepository.GetAllTypes();
        }

        public List<TipoModel> GetTypesByFilter(int? size, string? gender, string? characteristics)
        {
            var collectPoint = _utilsService.GetPontoColetaIdFromToken();

            var types = _tipoRepository.GetTypesByFilter(size, gender, characteristics, collectPoint);
            return types;
        }

        public TipoModel GetById(int id)
        {
            var type = _tipoRepository.GetById(id); 
            _ = type ?? throw new NotFoundException("Tipo do produto nao encontrado");
            return type;
        }
    }
}
