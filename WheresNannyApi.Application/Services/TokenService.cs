using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
        public TokenService(IRepository repository, IConfiguration configuration) 
        {
            _repository = repository;
            _configuration = configuration;
        }

        public string GenerateTokenBasedInUser(Person person, DateTime timeToExpire)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWt:Key"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", person.User.Id.ToString()),
                new Claim("imageUri", person.ImageUri),
                new Claim(JwtRegisteredClaimNames.Sub, person.User.Username),
                new Claim(JwtRegisteredClaimNames.Email, person.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = timeToExpire,
                SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        public async Task<string> Login(UserLoginDto user)
        {
            var userFounded = await _repository.GetAsync<User>(x => x.Username == user.Username && x.Password == user.Password);

            if (userFounded == null) return null;

            DateTime timeToExpire = DateTime.UtcNow.AddMinutes(5);

            var person = await _repository.GetAsync<Person>(x => x.UserId == userFounded.Id);

            var jwtToken = GenerateTokenBasedInUser(person, timeToExpire);

            userFounded.Token = jwtToken;
            userFounded.CreatedIn = DateTime.Now;
            userFounded.ExpiresIn = timeToExpire;

            _repository.Update(userFounded);
            await _repository.SaveChangesAsync();

            return jwtToken;
        }
    }
}
