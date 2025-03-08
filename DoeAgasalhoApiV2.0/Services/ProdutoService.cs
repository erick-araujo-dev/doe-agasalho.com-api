using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using System.Reflection.PortableExecutable;

namespace DoeAgasalhoApiV2._0.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly ITamanhoRepository _tamanhoRepository;
        private readonly IPontoColetaRepository _pontoColetaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITipoService _tipoService;
        private readonly ITamanhoService _tamanhoService;
        private readonly IUtilsService _utilsService;

        public ProdutoService
            (
            IProdutoRepository produtoRepository,
            ITipoRepository tipoRepository,
            ITamanhoRepository tamanhoRepository,
            IPontoColetaRepository pontoColetaRepository,
            IUsuarioRepository usuarioRepository,
            ITipoService tipoService,
            ITamanhoService tamanhoService,
            IUtilsService utilsService
            )
        {
            _produtoRepository = produtoRepository;
            _tipoRepository = tipoRepository;
            _tamanhoRepository = tamanhoRepository;
            _pontoColetaRepository = pontoColetaRepository;
            _usuarioRepository = usuarioRepository;
            _tipoService = tipoService;
            _tamanhoService = tamanhoService;
            _utilsService = utilsService;
        }

        //Obtem todos os produtos, se nao for selecionado nenhum filtro, se filtrado, retorna apenas os filtrados
        public List<ProdutoViewModel> GetAllOrFiltered(int? tipoId, int? tamanhoId, string? genero, string? caracteristica)
        {
            var collectPointId = _GetCollectPointIdAuth();

            var products = _produtoRepository.GetAllOrFiltered(tipoId, tamanhoId, genero, caracteristica, collectPointId);

            return products;
        }

        //Retorna as caracteristicas filtradas por produto e tamanho
        public List<string> GetCharacteristicsByFilter(int? tipoId, int? tamanhoId, string? genero)
        {
            var collectPointId = _GetCollectPointIdAuth();

            var characteristics = _produtoRepository.GetCharacteristicsByFilter(tipoId, tamanhoId, genero, collectPointId);
            return characteristics;
        }

        public List<ProdutoViewModel> GetProductsWithFilter(string type, string size, string gender, string characteristic)
        {
            var collectPointId = _GetCollectPointIdAuth();

            var products = _produtoRepository.GetWithFilter(type, size, gender,characteristic, collectPointId);
            return products;
        }

        //Retorna as generos filtradas por produto e tamanho
        public List<string> GetGenderByFilters(int? tipoId, int? tamanhoId, string? characteristics)
        {
            var collectPointId = _GetCollectPointIdAuth();

            var genders = _produtoRepository.GetGenderByFilter(tipoId, tamanhoId, characteristics, collectPointId);
            return genders;
        }

        //Retorna produto por Id
        public ProdutoViewModel GetProdutoById(int id)
        {
            var product = _produtoRepository.GetById(id);
            _ = product ?? throw new NotFoundException("Produto não encontrado.");

            _utilsService.VerifyProductAssociation(id);

            var produtoViewModel = new ProdutoViewModel
            {
                Id = product.Id,
                Ativo = product.Ativo,
                Tipo = product.Tipo.Nome,
                Caracteristica = product.Caracteristica,
                Tamanho = product.Tamanho.Nome,
                Genero = product.Genero,
                Estoque = product.Estoque
            };

            return produtoViewModel;
        }

        //Obter Produtos ativos
        public List<ProdutoViewModel> GetByActiveProdutos()
        {
            var collectPointId = _GetCollectPointIdAuth();

            // o no return "1", representa que vou fazer uma query onde o ativo for == 1, para retornar apenas os produtos ativos 
            return _produtoRepository.GetProdutosByPontoColeta(collectPointId, "1");
        }

        //Obter Produtos inativos
        public List<ProdutoViewModel> GetByInactiveProdutos()
        {
            var collectPointId = _GetCollectPointIdAuth();

            // o no return "0", representa que vou fazer uma query onde o ativo for == 0, para retornar apenas os produtos inativos 
            return _produtoRepository.GetProdutosByPontoColeta(collectPointId, "0");
        }

        //Obter todos (ativos e inativos)
        public List<ProdutoViewModel> GetAllProdutos()
        {
            var collectPointId = _GetCollectPointIdAuth();

            return _produtoRepository.GetProdutosByPontoColeta(collectPointId);
        }
       

        //criar um novo produto
        public ProdutoModel CreateProduct(ProdutoCreateModel newProductCreate)
        {
            var userId = _GetUserIdAuth();
            var collectPointId = _GetCollectPointIdAuth();

            //Valida caracteristica e genero
            _utilsService.ValidateStringField(newProductCreate.Caracteristica, "caracteristica", 50, true);
            _ValidateGender(newProductCreate.Genero);

            //Valida os dados, e cria um novo tipo e tamanho se necessario
            _utilsService.ValidateStringField(newProductCreate.Tipo, "tipo", 20, false);
            var existingType = _GetOrCreateTipo(newProductCreate.Tipo);

            _utilsService.ValidateStringField(newProductCreate.Tamanho, "tamanho", 20, true);
            var existingSize = _GetOrCreateTamanho(newProductCreate.Tamanho);

            var product = new ProdutoModel
            {
                TipoId = existingType.Id,
                TamanhoId = existingSize.Id,
                Caracteristica = newProductCreate.Caracteristica,
                Genero = newProductCreate.Genero,
            };

            var newProduct = _produtoRepository.Add(product, collectPointId, userId);
            return newProduct;
        }

        //retorna o id do user auth
        private int _GetUserIdAuth()
        {
            var userId = _utilsService.GetUserIdFromToken();
            var existingUser = _usuarioRepository.GetById(userId);
            if (existingUser == null || existingUser.Ativo == "0" || existingUser.Tipo == "admin")
            {
                throw new UnauthorizedAccessException("Você não tem permissão para adicionar um novo produto.");
            }

            return existingUser.Id;
        }

        //retorna o id ponto de coleta do user auth
        private int _GetCollectPointIdAuth()
        {
            var collectPointId = _utilsService.GetPontoColetaIdFromToken();
            var existingCollectPoint = _pontoColetaRepository.GetById(collectPointId);
            if (existingCollectPoint == null || existingCollectPoint.Ativo == "0")
            {
                throw new UnauthorizedAccessException("Ocorreu um erro ao tentar acessar o estoque do ponto de coleta");
            }

            return existingCollectPoint.Id;
        }

        public ProdutoModel UpdateProduct(int id, ProdutoCreateModel product)
        {
            var existingProduct = _produtoRepository.GetSingleByFilter(p => p.Id == id);

            _ = existingProduct ?? throw new NotFoundException("Produto não encontrado."); //Verifica se eh nulo, se sim, lanca uma exception

            //Valida os dados recebidos para atualizacao
            _utilsService.ValidateStringField(product.Caracteristica, "caracteristica", 50, true);
            _ValidateGender(product.Genero);
            _utilsService.ValidateStringField(product.Tamanho, "tamanho", 20, true);
            _utilsService.ValidateStringField(product.Tipo, "tipo", 20, false);

            //Se forem validos prossegue para o update
            existingProduct.Caracteristica = product.Caracteristica;
            existingProduct.Genero = product.Genero;

            var existingType = _GetOrCreateTipo(product.Tipo);
            existingProduct.TipoId = existingType.Id;

            var existingSize = _GetOrCreateTamanho(product.Tamanho);
            existingProduct.TamanhoId = existingSize.Id;

            _produtoRepository.Update(existingProduct);
            return existingProduct;
        }

        //Valida genero
        private void _ValidateGender(string gender)
        {
            if (!"MFU".Contains(gender.ToUpper()) || gender.Length > 1)
            {
                throw new ArgumentException("Digite apenas a inicial, M para Masculino, F para Feminino, U para Unissex");
            }
        }

        //Faz a busca e caso nao encontre cria um novo tipo
        private TipoModel _GetOrCreateTipo(string tipoName)
        {
            var existingType = _tipoRepository.GetByName(tipoName);

            if (existingType == null)
            {
                existingType = _tipoService.CreateNewType(tipoName);
            }

            return existingType;
        }
        //Faz a busca e caso nao encontre cria um novo tamanho
        private TamanhoModel _GetOrCreateTamanho(string tamanhoName)
        {
            var existingSize = _tamanhoRepository.GetByName(tamanhoName);

            if (existingSize == null)
            {
                existingSize = _tamanhoService.CreateNewSize(tamanhoName);
            }

            return existingSize;
        }

        public void ActivateProduct(int id)
        {
            var product = _produtoRepository.GetById(id);
            _ = product ?? throw new NotFoundException("Produto não encontrado.");

            if (product.Ativo == "1")
            {
                throw new InvalidOperationException("Produto ja esta ativo");
            }

            _produtoRepository.ActivateProduct(id);
        }

        public void DeactivateProduct(int id)
        {
            var product = _produtoRepository.GetById(id);
            _ = product ?? throw new NotFoundException("Produto não encontrado.");

            if (product.Ativo == "0")
            {
                throw new InvalidOperationException("Produto ja esta inativo");
            }

            _produtoRepository.DeactivateProduct(id);
        }



    }
}


