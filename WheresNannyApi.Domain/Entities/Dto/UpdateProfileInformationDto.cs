using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class UpdateProfileInformationDto
    {
        public PersonInformationDto PersonInformation { get; set; }
        public AddressFromUpdateInformationDto AddressFromUpdateInformation { get; set; }
    }

    public class PersonInformationDto
    {
        public string Fullname { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cellphone { get; set; } = string.Empty;
    }

    public class AddressFromUpdateInformationDto
    {
        public string Cep { get; set; } = string.Empty;
        public string Complement { get; set; } = string.Empty;
        public string Number { get; set; }
    }
}
