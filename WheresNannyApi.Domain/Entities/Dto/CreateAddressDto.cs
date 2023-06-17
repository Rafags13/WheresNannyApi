using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class CreateAddressDto
    {
        public string Cep { get; set; } = string.Empty;
        public string Complement { get; set; } = string.Empty;
        public string Number { get;set; } = string.Empty;
    }
}
