using Microsoft.AspNetCore.Mvc;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;

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

        [HttpGet("GetAllNannys")]
        public async Task<IActionResult> GetAllNannys()
        {
            var nannys = await _personService.GetAllNannys();

            if(nannys == null)
            {
                return NotFound("Não existe nenhuma babá registrada no sistema ainda.");
            }

            return Ok(nannys);
        }
    }
}
