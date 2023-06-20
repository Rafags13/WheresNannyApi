using Microsoft.AspNetCore.Mvc;
using WheresNannyApi.Application.Interfaces;

namespace WheresNannyApi.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NannyController : ControllerBase
    {
        private readonly INannyService _nannyService;
        public NannyController(INannyService nannyService)
        {
            _nannyService = nannyService;
        }

        [HttpGet("GetAllServices/{userId}")]
        public async Task<IActionResult> GetAllServices([FromRoute] int userId)
        {
            try
            {
                var services = await _nannyService.GetAllNannyServices(userId);

                if (services.Count == 0) return NotFound("Nenhum serviço foi encontrado.");

                return Ok(services);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
