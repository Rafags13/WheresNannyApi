﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class MainPayer
    {
        public int Id { get; set; }
        public int FirstServiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UriClient { get; set; } = string.Empty;
        public decimal TotalPayment { get; set; }
        public DateTime DateFromFirstHire { get; set; }
    }
}
