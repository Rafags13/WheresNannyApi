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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthenticationController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            var token = await _tokenService.Login(user);

            if(token == null) return NotFound($"Usuário: {user.Username} não foi encontrado no sistema.");

            return Ok(token);
        }
    }
}
