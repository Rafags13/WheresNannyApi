using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class ChangeNannyListByFilterDto
    {
        public string Filter { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
    }
}
