using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWork _unitOfWork;
        public TokenService(IRepository repository, IConfiguration configuration, IUnitOfWork unitOfWork) 
        {
            _repository = repository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public string GenerateTokenBasedInUser(Person person, DateTime timeToExpire)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWt:Key"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", person.User.Id.ToString()),
                new Claim("imageUri", person.ImageUri),
                new Claim("username", person.User.Username),
                new Claim(JwtRegisteredClaimNames.Email, person.Email),
                new Claim("cep", person.Address.Cep)
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

            var person = _unitOfWork
                .GetRepository<Person>()
                .GetPagedList(include: person => person.Include(x => x.Address).Include(x => x.User)).Items
                .Where(x => x.UserId == userFounded.Id)
                .FirstOrDefault();

            if (person == null) return null;

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
