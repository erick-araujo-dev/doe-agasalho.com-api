using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services;
using System.Linq;

namespace DoeAgasalhoApiV2._0.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbDoeagasalhov2Context _context;
        private readonly IPontoColetaRepository _pontoColetaRepository;


        public UsuarioRepository(DbDoeagasalhov2Context context, IPontoColetaRepository pontoColetaRepository)
        {
            _context = context;
            _pontoColetaRepository = pontoColetaRepository;
        }

        public UsuarioModel GetByEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
        }

        public List<UsuarioModel> GetAll()
        {
            return _context.Usuarios.ToList();
        }

      
        public List<UsuarioViewModel> GetByActiveStatus(bool ativo)
        {
            string ativoString = ativo ? "1" : "0";
            var usuarios = _context.Usuarios.Where(u => u.Ativo == ativoString).ToList();

            var usuariosViewModel = usuarios
                .OrderByDescending(u => u.Tipo)
                .Select(usuario => new UsuarioViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo,
                    Ativo = usuario.Ativo,
                    PontoColeta = _pontoColetaRepository.GetById(usuario.PontoColetaId)?.NomePonto ?? String.Empty
                })
                .ToList();

            return usuariosViewModel;
        }



        public UsuarioModel GetById(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public UsuarioModel GetByUserName(string name)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Nome == name);
        }

        public UsuarioModel GetByEmailAndPassword(string email, string password)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == password);
        }
                

        public UsuarioModel Add(UsuarioCreateModel usuario)
        {
            var novoUsuario = new UsuarioModel
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Tipo = usuario.Tipo,
                Ativo = "1",
                PontoColetaId = usuario.PontoColetaId
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            return novoUsuario;
        }

        public void Update(UsuarioModel usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public void ActivateUser(int userId)
        {
            var user = _context.Usuarios.Find(userId);
            if (user != null)
            {
                user.Ativo = "1";
                _context.SaveChanges();
            }
        }

        public void DeactivateUser(int userId)
        {
            var user = _context.Usuarios.Find(userId);
            if (user != null)
            {
                user.Ativo = "0";
                _context.SaveChanges();
            }
        }

        //metodo para o select no front retorna os usuarios por ponto de coleta
        public IEnumerable<UsuarioModel> GetUsuariosByPontoColetaId(int? pontoColetaId)
        {
            if (pontoColetaId.HasValue)
            {
                return _context.Usuarios
                    .Where(u => u.PontoColetaId == pontoColetaId)
                    .Where(u => u.Ativo == "1")
                    .Where(u => u.Tipo != "admin")
                    .ToList();
            }

            return _context.Usuarios.
                Where(u => u.Tipo != "admin")
                .Where(u => u.Ativo == "1")
                .ToList();
        }
    }
}
