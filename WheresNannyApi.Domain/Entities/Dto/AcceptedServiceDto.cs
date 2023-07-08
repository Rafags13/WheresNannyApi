using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class AcceptedServiceDto
    {
        public AcceptedServiceDto(int serviceId, bool accepted)
        {
            ServiceId = serviceId;
            Accepted = accepted;
        }
        public AcceptedServiceDto() { }
        public int ServiceId { get; set; }
        public bool Accepted { get; set; }
    }
}
