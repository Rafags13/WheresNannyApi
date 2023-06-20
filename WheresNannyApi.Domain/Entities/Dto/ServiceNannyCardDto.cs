using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class ServiceNannyCardDto
    {
        public ServiceNannyCardDto() { }
        public int ServiceId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime HiringDate { get; set; } = new DateTime();
        public decimal ServicePrice { get; set; }
    }
}
