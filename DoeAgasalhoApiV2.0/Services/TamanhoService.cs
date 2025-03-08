using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TamanhoService : ITamanhoService
    {
        private readonly ITamanhoRepository _tamanhoRepository;
        private readonly IUtilsService _utilsService;

        public TamanhoService(ITamanhoRepository tamanhoRepository, IUtilsService utilsService)
        {
            _tamanhoRepository = tamanhoRepository;
            _utilsService = utilsService;
        }

        public TamanhoModel CreateNewSize(string size)
        {
            var newSize = _tamanhoRepository.Add(size);
            return newSize;
        }
        public List<TamanhoModel> GetSizesByFilter(int? type, string? gender, string? characteristics)
        {
            var collectPoint = _utilsService.GetPontoColetaIdFromToken();

            var sizes = _tamanhoRepository.GetSizesByFilter(type, gender, characteristics, collectPoint);
            return sizes;
        }

        public TamanhoModel GetById(int id)
        {
            var size = _tamanhoRepository.GetById(id);
            _ = size ?? throw new NotFoundException("Tamanhp do produto nao encontrado");
            return size;
        }
    }
}
