using Microsoft.AspNetCore.Mvc;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServiceController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateService(CreateContractNannyDto createContractNannyDto)
        {
            try
            {
                var serviceCreatedAsSucessfull = await _servicesService.CreateService(createContractNannyDto);
                if (serviceCreatedAsSucessfull) {
                    return Ok("Serviço contratado com sucesso!");
                }

                return BadRequest("Algo de errado ocorreu durante a contratação da sua babá");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
