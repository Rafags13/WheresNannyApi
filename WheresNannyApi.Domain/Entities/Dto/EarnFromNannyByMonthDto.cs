using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class EarnFromNannyByMonthDto
    {
        public decimal TotalEarn { get; set; }
        public ICollection<MainPayer>? MainPeopleWhoHireHer { get; set; }
    }
}
