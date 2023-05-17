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
            var nannys = await _personService.GetUserMainPageInformation(findCommonUserServicesDto);

            if(nannys == null)
            {
                return BadRequest("Algo de errado aconteceu durante a pesquisa dos seus dados.");
            }

            return Ok(nannys);
        }
    }
}
