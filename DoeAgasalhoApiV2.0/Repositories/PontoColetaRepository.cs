using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories
{
    public class PontoColetaRepository : IPontoColetaRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public PontoColetaRepository(DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        public bool IsProdutoAssociated(int produtoId, int pontoColetaId)
        {
            return _context.PontoProdutos.Any(pp => pp.ProdutoId == produtoId && pp.PontoColetaId == pontoColetaId);
        }

        public PontoColetaModel GetById(int id)
        {
            return _context.PontoColeta.
                Include(p => p.Endereco).
                FirstOrDefault(p => p.Id == id);
        }

        //Sobrecarga para usar na usuarioRepository
        public PontoColetaModel GetById(int? id)
        {
            return _context.PontoColeta.
                Include(p => p.Endereco).
                FirstOrDefault(p => p.Id == id);
        }

        public List<PontoColetaViewModel> GetByActiveStatus(bool ativo)
        {
            string ativoString = ativo ? "1" : "0";
            return _context.PontoColeta.
                Include(u => u.Endereco).
                Where(u => u.Ativo == ativoString).
                Select(p => new PontoColetaViewModel
                {
                    Id = p.Id,
                    Ativo = ativoString,
                    NomePonto = p.NomePonto,
                    Logradouro = p.Endereco.Logradouro,
                    Numero = p.Endereco.Numero,
                    Complemento = p.Endereco.Complemento,
                    Bairro = p.Endereco.Bairro,
                    Cidade = p.Endereco.Cidade,
                    Estado = p.Endereco.Estado,
                    Cep = p.Endereco.Cep

                }).ToList();
        }

        public PontoColetaModel GetByName(string name)
        {
            return _context.PontoColeta.FirstOrDefault(u => u.NomePonto == name);
        }

        public async Task<List<PontoColetaViewModel>> GetAllActive()
        {
            var viewModels = await _context.PontoColeta
               .Where(u => u.Ativo == "1")
                .Select(p => new PontoColetaViewModel
                {
                    Id = p.Id,
                    Ativo = p.Ativo,
                    NomePonto = p.NomePonto,
                    Logradouro = p.Endereco.Logradouro,
                    Numero = p.Endereco.Numero,
                    Complemento = p.Endereco.Complemento,
                    Bairro = p.Endereco.Bairro,
                    Cidade = p.Endereco.Cidade,
                    Estado = p.Endereco.Estado,
                    Cep = p.Endereco.Cep,
                    QuantidadeUsuarios = _context.Usuarios.Where(u => u.Ativo == "1").Count(u => u.PontoColetaId == p.Id),
                    QuantidadeProdutos = _context.PontoProdutos
                        .Where(pp => pp.PontoColetaId == p.Id)
                        .Sum(pp => pp.Produto.Estoque)
                })
                .ToListAsync();

            return viewModels;
        }

        public List<PontoColetaModel> GetAllOrFilteredCollectPoint(int? collectPointId)
        {
            var query = _context.PontoColeta.AsQueryable();

            if (collectPointId.HasValue)
            {
                query = query.Where(pc => pc.Id == collectPointId);
            }

            var collectPoints = query.Where(pc => pc.Ativo == "1").ToList();

            return collectPoints;
        }

        public PontoColetaModel Add(PontoColetaCreateModel novoPontoColeta)
        {
            var pontoColeta = new PontoColetaModel
            {
                NomePonto = novoPontoColeta.NomePonto,
                Ativo = "1"
            };

            string complemento = string.IsNullOrWhiteSpace(novoPontoColeta.Complemento) ? null : novoPontoColeta.Complemento;

            var endereco = new EnderecoModel
            {
                Logradouro = novoPontoColeta.Logradouro,
                Numero = novoPontoColeta.Numero,
                Complemento = complemento,
                Bairro = novoPontoColeta.Bairro,
                Cidade = novoPontoColeta.Cidade,
                Estado = novoPontoColeta.Estado,
                Cep = novoPontoColeta.Cep
            };
            pontoColeta.Endereco = endereco;

            _context.PontoColeta.Add(pontoColeta);
            _context.SaveChanges();

            return pontoColeta;
        }

        public void Update(PontoColetaModel pontoColeta)
        {
            _context.PontoColeta.Update(pontoColeta);
            _context.SaveChanges();
        }

        public void ActivateCollectPoint(int id)
        {
            var pontoColeta = _context.PontoColeta.FirstOrDefault(p => p.Id == id);
            if (pontoColeta != null)
            {
                pontoColeta.Ativo = "1";
                _context.SaveChanges();
            }
        }

        public void DeactivateCollectPoint(int id)
        {
            var pontoColeta = _context.PontoColeta.FirstOrDefault(p => p.Id == id);
            if (pontoColeta != null)
            {
                pontoColeta.Ativo = "0";
                _context.SaveChanges();
            }
        }
    }
}
