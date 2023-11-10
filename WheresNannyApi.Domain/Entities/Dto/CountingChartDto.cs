﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class CountingChartDto
    {
        public CountingChartDto(int data, int monthIndex)
        {
            Data = data;
            MonthIndex = monthIndex;

        }
        public CountingChartDto() { }
        public int Data { get; set; }
        public int MonthIndex { get; set; }
    }
}
