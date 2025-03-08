using Microsoft.AspNetCore.Mvc;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repository.Interface;

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUtilsService _utilsService;


        public UsuarioController(IUsuarioService usuarioService, IUtilsService utilsService, IUsuarioRepository usuarioRepository)
        {
            _usuarioService = usuarioService;
            _utilsService = utilsService;   
            _usuarioRepository = usuarioRepository;
        }

        //Retorna os usuarios por ponto de coleta, se nao for enviado o ponto de coleta retorna todos usuarios
        [HttpGet]
        public IActionResult GetUsuariosByPontoColetaId(int? collectPointId)
        {
            try
            {
                var usuarios = _usuarioService.GetUserByCollectPoint(collectPointId);
                return Ok(usuarios);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários.");
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _usuarioService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter todos os usuários.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = _usuarioService.GetById(id);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao buscar o usuário.");
            }
        }

        [HttpGet("active")]
        [Authorize(Roles = "admin")]
        public IActionResult GetActiveUsers()
        {
            try
            {
                var users = _usuarioService.GetActiveUsers();
                return Ok(users);
            }
           
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários ativos.");
            }
        }

        [HttpGet("inactive")]
        [Authorize(Roles = "admin")]
        public IActionResult GetInactiveUsers()
        {
            try
            {
                var users = _usuarioService.GetInactiveUsers();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários inativos.");
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult<UsuarioModel> CreateNewUser(UsuarioCreateModel usuario)
        {
            try
            {
                var newUser = _usuarioService.CreateUser(usuario);
                return Ok(newUser);
            }
            catch (BusinessOperationException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o usuário.");
            }
        }

        [HttpPut("{id}/updateusername")]
        public ActionResult<UsuarioModel> UpdateUsername(int id, UpdateUsernameModel usuario)
        {
            try
            {
                var updatedUser = _usuarioService.UpdateUsername(id, usuario);
                return Ok(updatedUser);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro de autorização: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o usuário.");
            }
        }

        [HttpPut("{id}/changecollectpoint")]
        [Authorize(Roles = "admin")]
        public ActionResult<UsuarioModel> Change(int id, ChangeCollectPointModel usuario)
        {
            try
            {
                var updatedUser = _usuarioService.ChangeCollectPoint(id, usuario);
                return Ok(updatedUser);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o usuário.");
            }

        }

        [HttpPut("{id}/changepassword")]
        public IActionResult ChangePassword(int id, [FromBody] ChangePasswordModel user)
        {
            try
            {
                var updatedUser = _usuarioService.ChangePassword(id, user);
                return Ok(updatedUser);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, $"Erro de validacao: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro de autorização: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}/activate")]
        [Authorize(Roles = "admin")]
        public IActionResult ActivateUser(int id)
        {
            try
            {
                _usuarioService.ActivateUser(id);
                var response = new { success = true, message = "Usuário ativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro de autorização: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }

        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "admin")]
        public IActionResult DeactivateUser(int id)
        {
            try
            {
                _usuarioService.DeactivateUser(id);
                var response = new { success = true, message = "Usuário desativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro de autorização: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }
    }
}

