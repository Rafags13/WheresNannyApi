using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class UserRegisterDto
    {
        public string Fullname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Cellphone { get; set; } = string.Empty;
        public DateTime BirthdayDate { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string ImageUri { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string? HouseNumber { get; set; }
        public string? Complement { get; set;}
    }
}
