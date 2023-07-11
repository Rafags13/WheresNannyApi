using Microsoft.AspNetCore.Mvc;
using WheresNannyApi.Domain.Entities.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using WheresNannyApi.Application.Interfaces;

namespace WheresNannyApi.WebApi.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthenticationController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            try
            {
                var token = await _tokenService.Login(user);

                if(token == null) return NotFound("Usuário informado não foi encontrado no sistema ou a senha informada está incorreta.");

                return Ok(token);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] int userId)
        {
            try
            {
                var successfulToLogout = _tokenService.Logout(userId);

                if (!successfulToLogout)
                {
                    return BadRequest("Não foi possível atender à requisição. Tente Novamente.");
                }

                return Ok(successfulToLogout);

            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
