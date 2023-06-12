using Microsoft.AspNetCore.Mvc;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("GetUserHomeInformation")]
        public async Task<IActionResult> GetUserMainPageInformation(FindCommonUserServicesDto findCommonUserServicesDto)
        {
            try
            {
                var nannys = await _personService.GetUserMainPageInformation(findCommonUserServicesDto);

                if (nannys == null)
                {
                    return BadRequest("Algo de errado aconteceu durante a pesquisa dos seus dados.");
                }

                return Ok(nannys);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangeNannyListByFilter")]
        public IActionResult ChangeNannyListByFilter(ChangeNannyListByFilterDto changeNannyListByFilterDto)
        {
            try
            {
                var nannyList = _personService.NannyListOrderedByFilter(changeNannyListByFilterDto);

                if(nannyList == null)
                {
                    return BadRequest("Algo de errado aconteceu durante a listagem das babas. Feche e abra novamente o aplicativo..");
                }

                return Ok(nannyList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNannyById/{id}/{userId}")]
        public IActionResult GetNannyInfoToContractById([FromRoute] int id, int userId)
        {
            try
            {
                var nanny = _personService.GetNannyInfoToContractById(id, userId);

                if(nanny == null)
                {
                    return BadRequest("Não foi possível encontrar a babá requerida. Por favor, tente novamente.");
                }

                return Ok(nanny);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
