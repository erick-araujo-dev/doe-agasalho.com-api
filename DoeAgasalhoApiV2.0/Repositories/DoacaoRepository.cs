using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories
{
    public class DoacaoRepository : IDoacaoRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public DoacaoRepository(DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        public async Task<DoacaoModel> Add(int produtoId, int quantidade, int userId, string tipoMovimento)
        {
            DateTime data = DateTime.Now;

            var itemDoado = new DoacaoModel
            {
                ProdutoId = produtoId,
                Quantidade = quantidade,
                TipoMovimento = tipoMovimento,
                DataMovimento = data,
                UsuarioId = userId
            };

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produtoId);

            if (tipoMovimento == "entrada")
            {
                produto.Estoque += quantidade;
            }
            else if (tipoMovimento == "saida")
            {
                produto.Estoque -= quantidade;
            }

            _context.Set<DoacaoModel>().Add(itemDoado);
            await _context.SaveChangesAsync();

            return itemDoado;
        }

        public async Task<DoacaoModel> GetDoacao(int id)
        {
            return await _context.Doacoes.FindAsync(id);
        }

        public IQueryable<DoacaoModel> GetAll()
        {
            return _context.Doacoes
                    .Include(d => d.Usuario)
                    .Include(d => d.Produto);
        }


        public DoacaoModel GetById(int id)
        {
            return _context.Doacoes
                .Include(d => d.Usuario)
                .Include(d => d.Produto)
                .FirstOrDefault(d => d.Id == id);
        }
    }
}
