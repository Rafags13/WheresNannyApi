using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

        [HttpGet("GetServiceInformationsFromNanny/{serviceId}")]
        [Authorize]
        public IActionResult GetServiceInformationsFromNanny([FromRoute] int serviceId)
        {
            try
            {
                var serviceInformation = _servicesService.GetServiceInformations(serviceId, true);
                return Ok(serviceInformation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetServiceInformationsFromPerson/{serviceId}")]
        [Authorize]
        public IActionResult GetServiceInformationsFromPerson([FromRoute] int serviceId)
        {
            try
            {
                var serviceInformation = _servicesService.GetServiceInformations(serviceId, false);
                return Ok(serviceInformation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CancelService/{serviceId}")]
        [Authorize]
        public IActionResult CancelTheService([FromRoute] int serviceId)
        {
            try
            {
                var sucessful = _servicesService.CancelTheService(serviceId);

                if(!sucessful)
                {
                    return BadRequest("Não foi possível cancelar o serviço. Tente novamente, mais tarde");
                }

                return Ok("Serviço cancelado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
    }
}
