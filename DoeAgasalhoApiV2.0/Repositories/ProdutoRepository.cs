using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public ProdutoRepository(DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        //ativa um produto
        public void ActivateProduct(int productId)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Ativo = "1";
                _context.SaveChanges();
            }
        }

        //Desativa um produto
        public void DeactivateProduct(int productId)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Ativo = "0";
                _context.SaveChanges();
            }
        }

        //adiciona um novo produto, um novo relacionamento n n, e um novo registro na tabela de Doacoes
        public ProdutoModel Add(ProdutoModel product, int collectPointId, int userId)
        {
            //Cria um novo produto
            var newProduct = new ProdutoModel
            {
                TipoId = product.TipoId,
                TamanhoId = product.TamanhoId,
                Genero = product.Genero,
                Caracteristica = product.Caracteristica,
                Ativo = "1",
                Estoque = 0
            };

            _context.Produtos.Add(newProduct);
            _context.SaveChanges();

            //Cria a relacao do produto com o ponto de coleta
            var productPoint = new PontoProdutoModel
            {
                ProdutoId = newProduct.Id,
                PontoColetaId = collectPointId,
            };

            _context.PontoProdutos.Add(productPoint);
            _context.SaveChanges();

            //cria um registro de doacao na tabela de doacoes
            var DonationRegister = new DoacaoModel
            {
                ProdutoId = newProduct.Id,
                TipoMovimento = "cadastro",
                DataMovimento = DateTime.Now,
                Quantidade = 0,
                UsuarioId = userId
            };

            _context.Doacoes.Add(DonationRegister);
            _context.SaveChanges(); 

            return newProduct;
        }

        //faz um filtro retornando o primeiro produto encontrado, pode buscar por id, categoria, tamanho etc 
        public ProdutoModel GetSingleByFilter(Func<ProdutoModel, bool> filter)
        {
            return _context.Produtos.FirstOrDefault(filter);
        }

        //faz um filtro retornando o uma lista de produtos encontrado, pode buscar por id, categoria, tamanho etc 

        public List<ProdutoModel> GetAllByFilter(Func<ProdutoModel, bool> filter)
        {
            return _context.Produtos.Where(filter).ToList();
        }

        //filtro personalizado para usar na busca de produto, tem todas opcoes, porém só fará a busca, para os filstros selecionados
       

        public IQueryable<ProdutoModel> GetAll()
        {
            return _context.Produtos.
                Include(p => p.PontoProdutos);
        }

        public List<ProdutoViewModel> GetAllOrFiltered(int? tipoId, int? tamanhoId, string genero, string caracteristica, int collectPointIdAuth)
        {
            var collectPointId = collectPointIdAuth;

            var query = _context.Produtos.Include(p => p.Tipo).Include(p => p.Tamanho).AsQueryable();

            if (tipoId.HasValue)
                query = query.Where(p => p.TipoId == tipoId.Value);

            if (tamanhoId.HasValue)
                query = query.Where(p => p.TamanhoId == tamanhoId.Value);

            if (!string.IsNullOrEmpty(genero))
                query = query.Where(p => p.Genero == genero);

            if (!string.IsNullOrEmpty(caracteristica))
                query = query.Where(p => p.Caracteristica == caracteristica);

            if (collectPointId != null)
                query = query.Where(p => p.PontoProdutos.Any(pp => pp.PontoColetaId == collectPointId));

            var produtos = query.OrderByDescending(p => p.Estoque).ToList();

            var produtosViewModel = produtos.Select(p => new ProdutoViewModel
            {
                Id = p.Id,
                Ativo = p.Ativo,
                Tipo = p.Tipo?.Nome,
                Caracteristica = p.Caracteristica,
                Tamanho = p.Tamanho?.Nome,
                Genero = p.Genero,
                Estoque = p.Estoque
            }).ToList();

            return produtosViewModel;
        }

        //Lista pra buscar o produto antes de criar
        public List<ProdutoViewModel> GetWithFilter(
        string tipo,
        string tamanho,
        string genero,
        string caracteristica,
        int collectPointIdAuth)
        {
            var collectPointId = collectPointIdAuth;

            var query = _context.Produtos
                .Include(p => p.Tipo)
                .Include(p => p.Tamanho)
                .Include(p => p.PontoProdutos)
                .Where(p =>
                    (string.IsNullOrEmpty(tipo) || p.Tipo.Nome == tipo) &&
                    (string.IsNullOrEmpty(tamanho) || p.Tamanho.Nome == tamanho) &&
                    (string.IsNullOrEmpty(genero) || p.Genero == genero) &&
                    (string.IsNullOrEmpty(caracteristica) || p.Caracteristica == caracteristica) &&
                    (collectPointId == null || p.PontoProdutos.Any(pp => pp.PontoColetaId == collectPointId))
                )
                .ToList();

            var produtosViewModel = query.Select(p => new ProdutoViewModel
            {
                Id = p.Id,
                Ativo = p.Ativo,
                Tipo = p.Tipo?.Nome,
                Caracteristica = p.Caracteristica,
                Tamanho = p.Tamanho?.Nome,
                Genero = p.Genero,
                Estoque = p.Estoque
            }).ToList();

            return produtosViewModel;
        }

        //filtragem para usar na exibicao dos valores do select, quando marcar um tipo, retornara as caracteristicas para aquele tipo especifico
        public List<string> GetCharacteristicsByFilter(int? tipoId, int? tamanhoId, string genero, int collectPointId)
        {
            var query = _context.Produtos.AsQueryable();

            if (tipoId.HasValue)
                query = query.Where(p => p.TipoId == tipoId.Value);

            if (tamanhoId.HasValue)
                query = query.Where(p => p.TamanhoId == tamanhoId.Value);

            if (!string.IsNullOrEmpty(genero))
                query = query.Where(p => p.Genero == genero);

            if (collectPointId != null)
                query = query.Where(p => p.PontoProdutos.Any(pp => pp.PontoColetaId == collectPointId));

            var characteristics = query
                .Select(p => p.Caracteristica)
                .Distinct()
                .ToList();

            return characteristics;
        }

        public List<string> GetGenderByFilter(int? tipoId, int? tamanhoId, string? characteristics, int collectPointId)
        {
            var query = _context.Produtos.AsQueryable();

            if (tipoId.HasValue)
                query = query.Where(p => p.TipoId == tipoId.Value);

            if (tamanhoId.HasValue)
                query = query.Where(p => p.TamanhoId == tamanhoId.Value);

            if (!string.IsNullOrEmpty(characteristics))
                query = query.Where(p => p.Caracteristica == characteristics);

            if (collectPointId != null)
                query = query.Where(p => p.PontoProdutos.Any(pp => pp.PontoColetaId == collectPointId));

            var genders = query
                .Select(p => p.Genero)
                .Distinct()
                .ToList();

            return genders;
        }

        //Retorna os produtos de acordo com o ponto de coleta
        public List<ProdutoViewModel> GetProdutosByPontoColeta(int pontoColetaId, string ativo = null)
        {
            var query = _context.PontoProdutos
                .Where(pp => pp.PontoColetaId == pontoColetaId)
                .Include(pp => pp.Produto)
                .Select(pp => new ProdutoViewModel
                {
                    Id = pp.Produto.Id,
                    Tipo = pp.Produto.Tipo.Nome,
                    Ativo = pp.Produto.Ativo,
                    Caracteristica = pp.Produto.Caracteristica,
                    Tamanho = pp.Produto.Tamanho.Nome,
                    Genero = pp.Produto.Genero,
                    Estoque = pp.Produto.Estoque
                });

            if(ativo == "1")
            {
                query = query.Where(pp => pp.Ativo == "1");
            } 
            else if(ativo == "0")
            {
                query = query.Where(pp => pp.Ativo == "0");
            }

            var produtos = query.ToList();

            return produtos;
        }

        public ProdutoModel GetById(int produtoId)
        {
            var produto = _context.Produtos
                .Include(p => p.Tipo)
                .Include(p => p.Tamanho)
                .FirstOrDefault(p => p.Id == produtoId);

            return produto;
        }

        //atualiza um produto
        public void Update(ProdutoModel product)
        {
            _context.Produtos.Update(product);
            _context.SaveChanges();
        }
    }
}
