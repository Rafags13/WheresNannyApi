using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class FindCommonUserServicesDto
    {
        public int PersonId { get; set; }
        public string Cep { get; set; } = string.Empty;
    }
}
