using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TipoRepository : ITipoRepository
    {
        private readonly DbDoeagasalhov2Context _context;
        
        public TipoRepository (DbDoeagasalhov2Context context)
        {
            _context = context; 
        }

        public TipoModel Add(string typeName)
        {
            var newType = new TipoModel
            { 
                Nome = typeName,
                Ativo = "1"
            };
            _context.Tipos.Add(newType);
            _context.SaveChanges();
            return newType;
        }

        public TipoModel GetById(int id)
        {
            return _context.Tipos.FirstOrDefault(t => t.Id == id);  
        }

        public TipoModel GetByName(string name)
        {
            return _context.Tipos.FirstOrDefault(t => t.Nome == name);
        }

        public void Update(TipoModel model)
        {
            _context.Tipos.Update(model);
            _context.SaveChanges ();
        }

        
            public List<TipoModel> GetAllTypes()
        {
            return _context.Tipos.ToList();
        }

        public List<TipoModel> GetTypesByFilter(int? size, string? gender, string? characteristic, int collectpoint)
        {
            var query = _context.Produtos.AsQueryable();

            if (size.HasValue)
            {
                query = query.Where(p => p.TamanhoId == size.Value);
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

            var type = query
                .Select(p => p.TipoId)
                .Distinct()
                .ToList();

            var typesModels= _context.Tipos
                .Where(t => type.Contains(t.Id))
                .ToList();

            return typesModels;
        }
    }
}
