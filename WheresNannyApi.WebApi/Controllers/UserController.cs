using Microsoft.AspNetCore.Mvc;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
        {
            var errorMessage = await _userService.RegisterUser(userRegisterDto);

            if(errorMessage == "")
            {
                return Ok("Usuário registrado com sucesso!");
            }

            return Conflict(errorMessage);
        }

        [HttpPost("RegisterNanny")]
        public async Task<IActionResult> RegisterNanny([FromBody] NannyRegisterDto nannyRegisterDto)
        {
            var errorMessage = await _userService.RegisterNanny(nannyRegisterDto);

            if (errorMessage == "")
            {
                return Ok("Você foi registrada no sistema. Aguarde enquanto fazemos uma análise interna e, assim que for aprovada, te enviaremos um email com os próximos passos");
            }

            return Conflict(errorMessage);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                var errorMessage = await _userService.UpdatePassword(updatePasswordDto);

                if(errorMessage == "")
                {
                    return Ok("Senha Alterada com sucesso");
                }

                return BadRequest(errorMessage);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
