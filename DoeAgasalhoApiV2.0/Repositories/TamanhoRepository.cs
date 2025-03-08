using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TamanhoRepository : ITamanhoRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public TamanhoRepository (DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        public TamanhoModel Add(string sizeName)
        {
            var newSize = new TamanhoModel
            {
                Nome = sizeName,
            };
            _context.Tamanhos.Add(newSize);
            _context.SaveChanges();

            return newSize;
        }

        public TamanhoModel GetById(int id)
        {
            return _context.Tamanhos.FirstOrDefault(t => t.Id == id);
        }

        public TamanhoModel GetByName(string name)
        {
            return _context.Tamanhos.FirstOrDefault(t => t.Nome == name);
        }

        public void Update(TamanhoModel model)
        {
            _context.Tamanhos.Update(model);
            _context.SaveChanges();
        }

        public List<TamanhoModel> GetSizesByFilter(int? type, string gender, string characteristic, int collectpoint)
        {
            var query = _context.Produtos.AsQueryable();

            if (type.HasValue)
            {
                query = query.Where(p => p.TipoId == type.Value);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(p => p.Genero == gender);
            }

            if (!string.IsNullOrEmpty(characteristic))
            {
                query = query.Where(p => p.Caracteristica == characteristic);
            }

            if (collectpoint != null)
                query = query.Where(p => p.PontoProdutos.Any(pp => pp.PontoColetaId == collectpoint));

            var sizes = query
                .Select(p => p.TamanhoId)
                .Distinct()
                .ToList();

            var tamanhoModels = _context.Tamanhos
                .Where(t => sizes.Contains(t.Id))
                .ToList();

            return tamanhoModels;
        }

    }
}
