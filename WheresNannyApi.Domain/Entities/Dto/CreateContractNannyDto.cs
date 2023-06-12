using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class CreateContractNannyDto
    {
        public DateTime ServiceFinishHour { get; set; }
        public DateTime HiringDate { get; set; }
        public decimal Price { get; set; }
        public int PersonId { get; set; }
        public int NannyId { get; set; }
    }
}
