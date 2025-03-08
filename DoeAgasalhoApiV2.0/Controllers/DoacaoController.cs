using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/donations")]
    [ApiController]
    public class DoacaoController : ControllerBase
    {
        private readonly IDoacaoService _doacaoService;
        private readonly IDoacaoRepository _doacaoRepository;
        private readonly IUtilsService _utilsService;


        public DoacaoController(IDoacaoService doacaoService, IDoacaoRepository doacaoRepository, IUtilsService utilsService)
        {
            _doacaoService = doacaoService;
            _doacaoRepository = doacaoRepository;
            _utilsService = utilsService;
        }

        [HttpGet]
        public IActionResult GetDoacoes(int? collectPointId, int? userId, int? day, int? month, int? year, string? typeOfMovement)
        {
            var doacoes = _doacaoService.GetAllDonations(collectPointId, userId, day, month, year, typeOfMovement);
            return Ok(doacoes);
        }

        [HttpGet("{id}/search")]
        public ActionResult<DoacaoViewModel> GetDoacaoPorId(int id)
        {
            try
            {
                var resultado = _doacaoService.ExibirDoacaoPorId(id);
                return Ok(resultado);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao buscar a doação por ID. {ex.Message}");
            }
        }

        //Metodo usado para retornar o produto doado
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DoacaoModel>> Get(int id)
        {
            //ok
            try
            {
                var product = await _doacaoService.GetDoacao(id);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao buscar o produto.");
            }
        }

        [HttpPost("inventory/entry")]
        [Authorize(Roles = "normal")]
        public async Task<ActionResult<DoacaoModel>> PostEntradaDoacao([FromBody] DoacaoEntradaSaidaModel model)
        {
            //ok
            try
            {
                var itemDoado = await _doacaoService.DoacaoProduto(model, "entrada");
                return CreatedAtAction(nameof(Get), new { id = itemDoado.Id }, itemDoado);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao dar entrada em um novo produto.");
            }
        }

        [HttpPost("inventory/exit")]
        [Authorize(Roles = "normal")]
        public async Task<ActionResult<DoacaoModel>> PostSaidaDoacao([FromBody] DoacaoEntradaSaidaModel model)
        {
            //ok
            try
            {
                var itemDoado = await _doacaoService.DoacaoProduto(model, "saida");
                return CreatedAtAction(nameof(Get), new { id = itemDoado.Id }, itemDoado);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao dar entrada em um novo produto.");
            }
        }
    }
}
