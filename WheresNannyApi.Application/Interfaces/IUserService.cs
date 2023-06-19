using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Interfaces
{
    public interface IUserService
    {
        public Task<string> RegisterUser(UserRegisterDto userRegisterDto);
        public Task<string> RegisterNanny(NannyRegisterDto nannyRegisterDto);
        public Task<string> UpdatePassword(UpdatePasswordDto updatePasswordDto);
    }
}
