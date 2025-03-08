using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;
        private readonly IUtilsService _utilsService;
        private readonly IPontoColetaService _pontoColetaService;

        public LoginService(
            IUsuarioRepository usuarioRepository,
            IUsuarioService usuarioService,
            ITokenService tokenService,
            IPontoColetaService pontoColetaService,
            IUtilsService utilsService
            )
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _pontoColetaService = pontoColetaService;
            _utilsService = utilsService;
        }

        public UsuarioModel Authenticate(LoginModel model)
        {
            var usuario = _usuarioRepository.GetByEmailAndPassword(model.Email, model.Senha);

            //verifica se o login e senha sao validos 
            if (usuario == null || !_VerifyPassword(model.Senha, usuario.Senha))
            {
                throw new NotFoundException("Usuário ou senha inválidos");
            }

            // Verifica se o usuário está ativo
            if (!_usuarioService.IsActiveUser(usuario))
            {
                throw new InvalidOperationException("Usuário inativo, entre em contato com o administrador.");
            }

            // Verifica se o ponto de coleta do usuário está ativo, se estiver inativo não conseguirá fazer login
            if (!_utilsService.IsActiveCollectPoint(usuario) && usuario.Tipo != "admin")
            {
                throw new InvalidOperationException("Você não está associado a nenhum ponto de coleta, contate o administrador.");
            }

            return usuario;
        }

        private bool _VerifyPassword(string typedPassword, string storedPassword)
        {
            return typedPassword == storedPassword;
        }
    }
}
