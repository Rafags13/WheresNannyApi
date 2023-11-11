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
            if (userFounded is null)
            {
                throw new Exception("O usuário não foi encontrado no sistema ou a senha está incorreta.");
            }
                
            var currentPerson =
                _unitOfWork.GetRepository<Person>()
                .GetFirstOrDefault(
                    include: person =>
                    person.Include(x => x.Address)
                    .Include(x => x.User)
                    .Include(x => x.Nanny),
                    predicate: x => x.UserId == userFounded.Id);
  
            TypeOfUser typeOfUser = currentPerson.Nanny is not null ? TypeOfUser.Nanny : TypeOfUser.CommonUser;

            if (currentPerson == null) throw new Exception("Ocorreu um erro ao tentar carregar as informações pessoais. Contate um Administrador do sistema.;");

            DateTime timeToExpire = DateTime.UtcNow.AddMinutes(5); // Add session time in future

            GenerateTokenUserDto generateTokenUserDto = new(currentPerson, timeToExpire, user.DeviceId, typeOfUser);

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
            var issue = _configuration.GetValue<string>("JWt:Issuer");
            var audience = _configuration.GetValue<string>("JWt:Audience");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", generateTokenUserDto.PersonFromToken.User.Id.ToString()),
                new Claim("username", generateTokenUserDto.PersonFromToken.User.Username),
                new Claim(JwtRegisteredClaimNames.Email, generateTokenUserDto.PersonFromToken.Email),
                new Claim("cep", generateTokenUserDto.PersonFromToken.Address.Cep),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
                new Claim(JwtRegisteredClaimNames.Iss, issue),
                new Claim("deviceId", generateTokenUserDto.DeviceId),
                new Claim("typeOfUser", ((int) generateTokenUserDto.Type).ToString(), ClaimValueTypes.Integer)
             }),
                SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
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
            DateTime? nullableDateTime = null;

            currentUser.Token = "";
            currentUser.CreatedIn = nullableDateTime;
            currentUser.ExpiresIn = nullableDateTime;
            currentUser.DeviceId = "";

            _unitOfWork.GetRepository<User>().Update(currentUser);
            _unitOfWork.SaveChanges();

            return true;
        }
        #endregion
    }
}
