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

        [HttpGet("GetUserHomeInformation")]
        public async Task<IActionResult> GetUserMainPageInformation(int personId)
        {
            var nannys = await _personService.GetUserMainPageInformation(personId);

            if(nannys == null)
            {
                return BadRequest("Algo de errado aconteceu durante a pesquisa dos seus dados.");
            }

            return Ok(nannys);
        }
    }
}
