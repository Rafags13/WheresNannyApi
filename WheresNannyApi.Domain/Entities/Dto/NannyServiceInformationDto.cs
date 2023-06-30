using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class NannyServiceInformationDto
    {
        public NannyServiceInformationDto() { }

        public string ParentName { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public string NannyName { get; set; } = string.Empty;
        public float NannyCountStars { get; set; }
        public int NannyId { get; set; }
    }
}
