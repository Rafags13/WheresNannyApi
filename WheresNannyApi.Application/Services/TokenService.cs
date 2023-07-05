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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Application.Util;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

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
        #region Login User
        public async Task<string> Login(UserLoginDto user)
        {
            var passwordEncrypted = Functions.Sha1Encrypt(user.Password);
            var userFounded = await _repository.GetAsync<User>(x => x.Username == user.Username && x.Password == passwordEncrypted);

            if (userFounded == null) return null;

            DateTime timeToExpire = DateTime.UtcNow.AddMinutes(5);

            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    {"mensagem", $"Um novo serviço de {userFounded.Username} foi chamado, deseja aceitar?" },
                },
                Token = "e01j-vj1TB27eeQ00GJ18a:APA91bGo4t13G0k8PWoxmnUJAs819xRMF2Nl-XeYZ-bYeRoD9cnTqLUP5Dliiiwvd_bgi6-aKBC67Mwkor9wNYd4DJhMLkA_kA0IaUZXR9JwlFY5hJ7Oo_mKWqOxSe7rfvBDeDX-x9qd",
                Notification = new Notification()
                {
                    Title = "Test from code",
                    Body = "Body of message is here",
                },
            };

            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

            var person = _unitOfWork
                .GetRepository<Person>()
                .GetPagedList(include: person => person.Include(x => x.Address).Include(x => x.User).Include(x => x.Nanny)).Items
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
                new Claim("cep", person.Address.Cep),
                new Claim("isNanny", person.Nanny is not null ? "true" : "false", ClaimValueTypes.Boolean)
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
#endregion
    }
}
