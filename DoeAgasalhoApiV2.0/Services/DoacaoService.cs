using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using System.Runtime.Intrinsics.Arm;

namespace DoeAgasalhoApiV2._0.Services
{
    public class DoacaoService : IDoacaoService
    {
        private readonly IDoacaoRepository _doacaoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUtilsService _utilsService;
        private readonly ITipoService _tipoService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IPontoColetaRepository _pontoColetaRepository;
        private readonly ITamanhoService _tamanhoService;
        private readonly IPontoColetaService _pontoColetaService;

        public DoacaoService
        (
            IDoacaoRepository doacaoRepository,
            IProdutoRepository produtoRepository,
            IUtilsService utilsService,
            IUsuarioRepository usuarioRepository,
            IPontoColetaRepository pontoColetaRepository,
            ITipoService tipoService,
            ITamanhoService tamanhoService,
            IUsuarioService usuarioService,
            IPontoColetaService pontoColetaService)
        {
            _doacaoRepository = doacaoRepository;
            _produtoRepository = produtoRepository;
            _utilsService = utilsService;
            _usuarioRepository = usuarioRepository;
            _pontoColetaRepository = pontoColetaRepository;
            _tipoService = tipoService;
            _tamanhoService = tamanhoService;
            _usuarioService = usuarioService;
            _pontoColetaService = pontoColetaService;   
        }

        public List<DoacaoViewModel> GetAllDonations(int? collectPointId, int? userId, int? day, int? month, int? year, string? typeOfMovement)
        {
            var query = _doacaoRepository.GetAll();

            if (collectPointId.HasValue)
            {
                query = query.Where(d => d.Produto.PontoProdutos.Any(pp => pp.PontoColetaId == collectPointId));
            }

            if (userId.HasValue)
            {
                query = query.Where(d => d.UsuarioId == userId);
            }

            if (day.HasValue)
            {
                query = query.Where(d => d.DataMovimento.Day == day);
            }

            if (month.HasValue)
            {
                query = query.Where(d => d.DataMovimento.Month == month);
            }

            if (year.HasValue)
            {
                query = query.Where(d => d.DataMovimento.Year == year);
            }

            if (!string.IsNullOrEmpty(typeOfMovement))
            {
                query = query.Where(d => d.TipoMovimento == typeOfMovement);
            }

            var groupedDoacoes = query
    .GroupBy(d => new
    {
        d.Produto.TipoId,
        d.Produto.TamanhoId,
        d.TipoMovimento,
        d.Produto.Caracteristica,
        Data = new DateTime(d.DataMovimento.Year, d.DataMovimento.Month, d.DataMovimento.Day),
        d.UsuarioId,
        d.Usuario.PontoColetaId
})
    .Select(g => new
    {
        TipoId = g.Key.TipoId,
        TamanhoId = g.Key.TamanhoId,
        TipoMovimento = g.Key.TipoMovimento,
        Caracteristica = g.Key.Caracteristica,
        DataMovimento = g.Key.Data,
        UsuarioId = g.Key.UsuarioId,
        PontoColeta = g.Key.PontoColetaId,
        QuantidadeTotal = g.Sum(d => d.Quantidade)
    })
    .OrderBy(g => g.TipoMovimento == "entrada" ? 0 : g.TipoMovimento == "saida" ? 1 : 2)
    .ToList();
            var doacoesViewModel = groupedDoacoes.Select(g => new DoacaoViewModel
            {
                Tipo = _tipoService.GetById(g.TipoId)?.Nome,
                Tamanho = _tamanhoService.GetById(g.TamanhoId)?.Nome,
                Caracteristica = g.Caracteristica,
                TipoMovimento = g.TipoMovimento,
                Quantidade = g.QuantidadeTotal,
                DataMovimento = g.DataMovimento,
                Usuario = _usuarioService.GetById(g.UsuarioId)?.Nome,
                PontoColeta = g.PontoColeta.HasValue ? _pontoColetaService.GetById(g.PontoColeta.Value)?.NomePonto : null
            }).ToList();

            return doacoesViewModel;
        }



        public DoacaoViewModel ExibirDoacaoPorId(int id)
        {
            var doacao = _doacaoRepository.GetById(id);
            _ = doacao ?? throw new NotFoundException("Doação não encontrada");


            var doacaoViewModel = new DoacaoViewModel
            {
                Id = doacao.Id,
                Usuario = doacao.Usuario.Nome,
                Tipo = _tipoService.GetById(doacao.Produto.TipoId).Nome,
                Caracteristica = doacao.Produto.Caracteristica,
                TipoMovimento = doacao.TipoMovimento,
                Quantidade = doacao.Quantidade,
                DataMovimento = doacao.DataMovimento
            };

            return doacaoViewModel;
        }

        public List<DoacaoViewModel> FiltrarDoacoes(int mes, int usuarioId, string tipoMovimento)
        {
            var doacoes = _doacaoRepository.GetAll();

            // Filtrar por mês
            if (mes > 0)
            {
                doacoes = doacoes.Where(d => d.DataMovimento.Month == mes);
            }

            // Filtrar por usuário
            if (usuarioId > 0)
            {
                doacoes = doacoes.Where(d => d.UsuarioId == usuarioId);
            }

            // Filtrar por tipo de movimento
            if (!string.IsNullOrEmpty(tipoMovimento))
            {
                doacoes = doacoes.Where(d => d.TipoMovimento == tipoMovimento);
            }

            var doacoesViewModel = new List<DoacaoViewModel>();

            foreach (var doacao in doacoes)
            {
                var doacaoViewModel = new DoacaoViewModel
                {
                    Usuario = doacao.Usuario.Nome,
                    Tipo = _tipoService.GetById(doacao.Produto.TipoId)?.Nome,
                    Caracteristica = doacao.Produto.Caracteristica,
                    TipoMovimento = doacao.TipoMovimento,
                    Quantidade = doacao.Quantidade, // Trata o valor nulo da quantidade
                    DataMovimento = doacao.DataMovimento
                };

                doacoesViewModel.Add(doacaoViewModel);
            }

            return doacoesViewModel;
        }


        //

        public async Task<DoacaoModel> GetDoacao(int id)
        {
            var doacao = _doacaoRepository.GetDoacao(id);
            _ = doacao ?? throw new NotFoundException("Doação não encontrada.");

            return await doacao;
        }

        //Faz entrada ou saida de um produto
        public async Task<DoacaoModel> DoacaoProduto(DoacaoEntradaSaidaModel model, string tipoMovimento)
        {
            var userAuth = _utilsService.GetUserIdFromToken();

            var product = _produtoRepository.GetById(model.ProdutoId); //verifica se o produto existe
            _ = product ?? throw new NotFoundException("Produto não encontrado.");

            _utilsService.VerifyProductAssociation(model.ProdutoId); //Se o usuario nao estiver associado ao ponto coleta do produto, sera lancado um 404

            if (tipoMovimento == "entrada" && product.Ativo == "0") //verifica se o produto esta ativo, se nao, deixara fazer apenas saidas
            {
                throw new UnauthorizedAccessException("Erro ao fazer entrada, esse produto encontra-se inativo.");
            }

            if (model.Quantidade <= 0) //Verifica se digitou uma quantidade valida
            {
                throw new ArgumentException("Quantidade inválida, escolha um valor maior que 0.");
            }

            if (tipoMovimento == "saida" && product.Estoque < model.Quantidade)//verifica se a quantidasde eh menor ou igual ao estoque
            {
                throw new ArgumentException("Não há produtos o suficiente em estoque.");
            }

            // Chamar o método Add do DoacaoRepository para salvar a nova doação
            var doacao = await _doacaoRepository.Add(model.ProdutoId, model.Quantidade, userAuth, tipoMovimento);

            return doacao;
        }
    }
}
