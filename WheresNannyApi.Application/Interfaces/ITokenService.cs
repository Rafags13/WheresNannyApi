using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Interfaces
{
    public interface ITokenService
    {
        public Task<string> Login(UserLoginDto user);
        public string GenerateTokenBasedInUser(GenerateTokenUserDto generateTokenUserDto);
        public bool Logout(int userId);
    }
}
