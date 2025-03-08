using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Services;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ITipoService _tipoService;
        private readonly ITamanhoService _tamanhoService;
        private readonly IUtilsService _utilsService;


        public ProdutoController(IProdutoService produtoService, IUtilsService utilsService, ITipoService tipoService, ITamanhoService tamanhoService)
        {
            _produtoService = produtoService;
            _utilsService = utilsService;
            _tipoService = tipoService;
            _tamanhoService = tamanhoService;       
        }
        // GET: api/produtos
        [HttpGet]
        [Authorize(Roles = "normal")]
        public IActionResult GetAllOrFiltered(int? type, int? size, string? gender, string? characteristic)
        {
            try
            {
                var produtos = _produtoService.GetAllOrFiltered(type, size, gender, characteristic);
                return Ok(produtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao buscar os produtos. {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //ok
            try
            {
                var product = _produtoService.GetProdutoById(id);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro: {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao buscar o produto.");
            }
        }

        [HttpGet("active")]
        [Authorize(Roles = "normal")]
        public IActionResult GetActiveUsers()
        {
            //ok
            try
            {
                var products = _produtoService.GetByActiveProdutos();
                return Ok(products);
            }

            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os produtos ativos.");
            }
        }

        [HttpGet("inactive")]
        [Authorize(Roles = "normal")]
        public IActionResult GetInactiveUsers()
        {            
            //ok
            try
            {
                var products = _produtoService.GetByInactiveProdutos();
                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os produtos inativos.");
            }
        }

        [HttpGet("types")]
        [Authorize(Roles = "normal")]
        public IActionResult GetTypes(int? size, string? gender, string? characteristic)
        {
            try
            {
                var types = _tipoService.GetTypesByFilter(size, gender, characteristic);
                return Ok(types);
            }
            catch (Exception)
            {
                return StatusCode(500, "correu um erro ao buscar os tipos");
            }
        }

        [HttpGet("sizes")]
        [Authorize(Roles = "normal")]
        public IActionResult GetSizes(int? type, string? gender, string? characteristic)
        {
            try
            {
                var sizes = _tamanhoService.GetSizesByFilter(type, gender, characteristic);
                return Ok(sizes);
            }
            catch (Exception)
            {
                return StatusCode(500, "correu um erro ao buscar os tamanhos");
            }
        }
        [HttpGet("characteristics")]
        [Authorize(Roles = "normal")]
        public IActionResult GetCharacteristics(int? type, int? size, string? gender)
        {
            try
            {
                var characteristics = _produtoService.GetCharacteristicsByFilter(type, size, gender);
                return Ok(characteristics);
            }
            catch (Exception)
            {
                return StatusCode(500, "correu um erro ao buscar os caracteristicas");
            }
        }

        [HttpGet("genders")]
        [Authorize(Roles = "normal")]
        public IActionResult GetGenders(int? type, int? size, string? characteristic)
        {
            try
            {
                var genders = _produtoService.GetGenderByFilters(type, size, characteristic);
                return Ok(genders);
            }
            catch (Exception)
            {
                return StatusCode(500, "correu um erro ao buscar os generos");
            }
        }

        [HttpGet("filter")]
        [Authorize(Roles = "normal")]
        public IActionResult GetWirhFilter(string type, string size, string gender, string characteristic)
        {
            
            try
            {
                var products = _produtoService.GetProductsWithFilter(type, size, gender, characteristic);
                return Ok(products);
            }

            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os produtos ativos.");
            }
        }

        // POST api/produtos
        [HttpPost("create")]
        public ActionResult<ProdutoModel> Create(ProdutoCreateModel product)
        {
            //ok
            try
            {
                var newProduct = _produtoService.CreateProduct(product);
                return Ok(newProduct);
            }
            catch(UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro: {ex.Message}");
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
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }


        // PUT api/produtos

        [HttpPut("{id}/updateproduct")]
        public ActionResult<ProdutoModel> Update(int id, ProdutoCreateModel product)
        {
            //ok
            try
            {
                var updateProduct = _produtoService.UpdateProduct(id, product);
                return Ok(updateProduct);
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
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}/activate")]
        [Authorize(Roles = "admin")]
        public IActionResult ActivateUser(int id)
        {
            try
            {
                _produtoService.ActivateProduct(id);
                var response = new { success = true, message = "Produto ativado com sucesso!" };
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
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o produto.");
            }
        }

        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "admin")]
        public IActionResult DeactivateUser(int id)
        {
            try
            {
                _produtoService.DeactivateProduct(id);
                var response = new { success = true, message = "Produto desativado com sucesso!" };
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
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o produto.");
            }
        }
    }
}
