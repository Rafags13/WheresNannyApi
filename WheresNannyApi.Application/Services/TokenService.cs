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

            var person = _unitOfWork
                .GetRepository<Person>()
                .GetPagedList(include: person => person.Include(x => x.Address).Include(x => x.User).Include(x => x.Nanny)).Items
                .Where(x => x.UserId == userFounded.Id)
                .FirstOrDefault();

            if (person == null) return null;

            DateTime timeToExpire = DateTime.UtcNow.AddMinutes(5);

            GenerateTokenUserDto generateTokenUserDto = new GenerateTokenUserDto(person, timeToExpire, user.DeviceId);

            var jwtToken = GenerateTokenBasedInUser(generateTokenUserDto);

            userFounded.Token = jwtToken;
            userFounded.CreatedIn = DateTime.Now;
            userFounded.ExpiresIn = timeToExpire;

            userFounded.DeviceId = user.DeviceId;

            _repository.Update(userFounded);
            await _repository.SaveChangesAsync();

            return jwtToken;
        }

        public string GenerateTokenBasedInUser(GenerateTokenUserDto generateTokenUserDto)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWt:Key"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", generateTokenUserDto.PersonFromToken.User.Id.ToString()),
                new Claim("imageUri", generateTokenUserDto.PersonFromToken.ImageUri),
                new Claim("username", generateTokenUserDto.PersonFromToken.User.Username),
                new Claim(JwtRegisteredClaimNames.Email, generateTokenUserDto.PersonFromToken.Email),
                new Claim("cep", generateTokenUserDto.PersonFromToken.Address.Cep),
                new Claim("deviceId", generateTokenUserDto.DeviceId),
                new Claim("isNanny", generateTokenUserDto.PersonFromToken.Nanny is not null ? "true" : "false", ClaimValueTypes.Boolean)
             }),
                Expires = generateTokenUserDto.TimeToExpire,
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

        #region Logout
        public bool Logout(int userId)
        {
            var currentUser = _unitOfWork.GetRepository<User>().GetFirstOrDefault(predicate: x => x.Id == userId);

            if (currentUser is null) return false;

            currentUser.Token = "";
            currentUser.CreatedIn = null;
            currentUser.ExpiresIn = null;
            currentUser.DeviceId = "";

            _unitOfWork.GetRepository<User>().Update(currentUser);
            _unitOfWork.SaveChanges();

            return true;
        }
        #endregion
    }
}
