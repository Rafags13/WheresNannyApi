using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class RecentCardDto
    {
        public int ServiceId { get; set; }
        public string ImageUri { get; set; } = string.Empty;
        public string PersonName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
