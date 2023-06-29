using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class EarnCountingChartDto
    {
        public EarnCountingChartDto(float data)
        {
            Data = data;
        }
        public EarnCountingChartDto() { }

        public float Data { get; set; }
    }
}
