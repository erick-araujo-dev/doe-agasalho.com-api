using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/collectpoint")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class PontoColetaController : ControllerBase
    {
        private readonly IPontoColetaService _pontoColetaService;
        private readonly IPontoColetaRepository _pontoColetaRepository;

        public PontoColetaController(IPontoColetaService pontoColetaService, IPontoColetaRepository pontoColetaRepository)
        {
            _pontoColetaService = pontoColetaService;
            _pontoColetaRepository = pontoColetaRepository; 
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var pontosColeta = await _pontoColetaService.GetAllActive();
                return Ok(pontosColeta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao obter todos os pontos de coleta.{ex.Message}");
            }
        }

        //Retorna os usuarios por ponto de coleta, se nao for enviado o ponto de coleta retorna todos usuarios
        [HttpGet("byuser")]
        public IActionResult GetUsuariosByPontoColetaId(int? collectPointId)
        {
            try
            {
                var collectPoint = _pontoColetaService.GetAllOrFilteredCollectPoint(collectPointId);
                return Ok(collectPoint);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os pontos de coletas.");
            }
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var collectPoint = _pontoColetaService.GetById(id);
                return Ok(collectPoint);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao buscar o ponto de coleta.");
            }
        }

        [HttpGet("inactive")]
        public IActionResult GetInactive()
        {
            try
            {
                var users = _pontoColetaService.GetInactiveCollectPoint();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários inativos.");
            }
        }



        [HttpPost("create")]
        public ActionResult<PontoColetaModel> Create(PontoColetaCreateModel novoPontoColeta)
        {
            try
            {
                var pontoColeta = _pontoColetaService.CreateCollectPoint(novoPontoColeta);
                return Ok(pontoColeta);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao cadastrar o ponto de coleta."); ;
            }
        }



        [HttpPut("{id}/update")]
        public ActionResult<PontoColetaModel> Update(int id, PontoColetaCreateModel updatePontoColeta)
        {
            try
            {
                var pontoColeta = _pontoColetaService.UpdateCollectPoint(id, updatePontoColeta);
                return Ok(pontoColeta);

            } catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BusinessOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao atualizar o usuário.");
            }
        }

        [HttpPut("{id}/activate")]
        public IActionResult Activate(int id)
        {
            try
            {
                _pontoColetaService.ActivateCollectPoint(id);
                var response = new { success = true, message = "Ponto de coleta ativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }

        [HttpPut("{id}/deactivate")]
        public IActionResult Deactivate(int id)
        {
            try
            {
                _pontoColetaService.DeactivateCollectPoint(id);
                var response = new { success = true, message = "Ponto de coleta desativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);  
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }
    }
}
