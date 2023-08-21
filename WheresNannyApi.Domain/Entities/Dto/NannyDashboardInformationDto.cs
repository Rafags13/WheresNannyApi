using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class NannyDashboardInformationDto
    {
        public NannyDashboardInformationDto() { }
        public ServiceNannyCardDto? LastService { get; set; }
        public List<string> MonthNames { get; set; } = new List<string>();
        public List<CountingChartDto> CountingServiceChart { get; set; } = new List<CountingChartDto>();
        public List<EarnCountingChartDto> EarnCountingChart{ get; set; } = new List<EarnCountingChartDto>();
    }
}
