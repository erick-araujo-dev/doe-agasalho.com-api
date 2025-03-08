using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Services.Interface;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Exceptions;

namespace DoeAgasalhoApiV2._0.Services
{
    public class PontoColetaService : IPontoColetaService
    {
        private readonly IPontoColetaRepository _pontoColetaRepository;
        private readonly IEnderecoService _enderecoService;
        private readonly IUtilsService _utilsService;

        public PontoColetaService(IPontoColetaRepository pontoColetaRepository, IEnderecoService enderecoService, IUtilsService utilsService)
        {
            _pontoColetaRepository = pontoColetaRepository;
            _enderecoService = enderecoService;
            _utilsService = utilsService;
        }

        //Obter todos pontos de 
        public async Task<List<PontoColetaViewModel>> GetAllActive()
        {
            var viewModel = await _pontoColetaRepository.GetAllActive();
            return viewModel;
        }

        //Obter pontos de coleta ativos
        public List<PontoColetaViewModel> GetActivateCollectPoint() => _pontoColetaRepository.GetByActiveStatus(true);

        //Obter pontos de coletas desativados
        public List<PontoColetaViewModel> GetInactiveCollectPoint() => _pontoColetaRepository.GetByActiveStatus(false);

        //Obter por id
        public PontoColetaModel GetById(int id)
        {
            var collectPoint = _pontoColetaRepository.GetById(id);
            _ = collectPoint ?? throw new NotFoundException("Ponto de Coleta não encontrado.");

            return collectPoint;
        }

        public List<PontoColetaModel> GetAllOrFilteredCollectPoint(int? collectPointId)
        {
            return _pontoColetaRepository.GetAllOrFilteredCollectPoint(collectPointId);
        }
        public PontoColetaModel CreateCollectPoint(PontoColetaCreateModel novoPontoColeta)
        {
            _ValidateCollectPointName(novoPontoColeta.NomePonto);
            _ValidateDataAddressCollectPoint(novoPontoColeta);

            return _pontoColetaRepository.Add(novoPontoColeta);
        }

        //Update username Ponto de Coleta 
        public PontoColetaModel UpdateCollectPoint(int id, PontoColetaCreateModel pontoColeta)
        {
            var collectPoint = _pontoColetaRepository.GetById(id);
            _ = collectPoint ?? throw new NotFoundException("Ponto de Coleta não encontrado.");

            _ValidateDataAddressCollectPoint(pontoColeta);
            if (pontoColeta.NomePonto != collectPoint.NomePonto)
            {
                collectPoint.NomePonto = pontoColeta.NomePonto;
                _ValidateCollectPointName(pontoColeta.NomePonto);
            }
            
            if (collectPoint.Endereco != null)
            {
                collectPoint.Endereco.Logradouro = pontoColeta.Logradouro;
                collectPoint.Endereco.Numero = pontoColeta.Numero;
                collectPoint.Endereco.Complemento = pontoColeta.Complemento;
                collectPoint.Endereco.Bairro = pontoColeta.Bairro;
                collectPoint.Endereco.Cidade = pontoColeta.Cidade;
                collectPoint.Endereco.Cep = pontoColeta.Cep;
            }

            _pontoColetaRepository.Update(collectPoint);
            return collectPoint;
        }

        //Valida endereco
        private void _ValidateDataAddressCollectPoint(PontoColetaCreateModel pontoColeta)
        {

            _enderecoService.ValidateAddress(
                pontoColeta.Numero,
                pontoColeta.Logradouro,
                pontoColeta.Complemento,
                pontoColeta.Bairro,
                pontoColeta.Cidade,
                pontoColeta.Estado,
                pontoColeta.Cep
                );

            //Abrevia e valida o estado
            pontoColeta.Estado = _enderecoService.AbbreviateState(pontoColeta.Estado);
            _enderecoService.ValidateStateName(pontoColeta.Estado);

            //formata o cep
            pontoColeta.Cep = _enderecoService.FormatZipCode(pontoColeta.Cep);
        }

        //Ativar pc
        public void ActivateCollectPoint(int id)
        {
            var collectPoint = _pontoColetaRepository.GetById(id);
            _ = collectPoint ?? throw new NotFoundException("Ponto de Coleta não encontrado.");

            _utilsService.ValidateActive(collectPoint.Ativo);

            if (collectPoint.Ativo == "1")
            {
                throw new InvalidOperationException("Ponto de coleta já está ativo.");
            }

            _pontoColetaRepository.ActivateCollectPoint(id);
        }


        //Desativar pc
        public void DeactivateCollectPoint(int id)
        {
            var collectPoint = _pontoColetaRepository.GetById(id);

            _ = collectPoint ?? throw new NotFoundException("Ponto de Coleta não encontrado.");

            _utilsService.ValidateActive(collectPoint.Ativo);

            if (collectPoint.Ativo == "0")
            {
                throw new InvalidOperationException("Ponto de coleta já está inativo.");
            }

            _pontoColetaRepository.DeactivateCollectPoint(id);
        }

        //Valida nome do ponto de coleta
        private void _ValidateCollectPointName(string name)
        {
            var user = _pontoColetaRepository.GetByName(name);

            if (user != null)
            {
                throw new BusinessOperationException("O nome fornecido já está sendo utilizado por outra unidade. Por favor, utilize outro nome.");
            }

            if (name.Length > 50)
            {
                throw new ArgumentException("O nome deve ter no máximo 50 caracteres.");
            }
        }
    }
}