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
                if (!serviceCreatedAsSucessfull) {
                    return BadRequest("Algo de errado ocorreu durante a contratação da sua babá");
                }

                return Ok("Tudo certo! A soliticação do serviço foi efetuada para a babá. Aguarde a resposta dela para prosseguir ou não com o serviço.");

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll/{userId}/{pageIndex}")]
        public IActionResult GetServiceById([FromRoute] int userId, int pageIndex)
        {
            try
            {
                var allServicesByUser = _servicesService.ListAllServices(userId, pageIndex);
                if (allServicesByUser is null)
                {
                    return NotFound("Não foi possível localizar nenhum serviço");
                }

                return Ok(allServicesByUser);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNannyServiceInformation/{serviceId}")]
        public IActionResult GetNannyServiceInformation([FromRoute] int serviceId)
        {
            try
            {
                var nannyServiceInformation = _servicesService.GetNannyServiceInformation(serviceId);
                if(nannyServiceInformation is null)
                {
                    return NotFound("O serviço selecionado não existe mais");
                }
                return Ok(nannyServiceInformation);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ServiceHasBeenAcceptedByNanny")]
        public IActionResult ServiceHasBeenAcceptedByNanny([FromBody] AcceptedServiceDto acceptedServiceDto)
        {
            try
            {
                _servicesService.ServiceAccepted(acceptedServiceDto);

                return Ok("O serviço foi aceito com sucesso");
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
