﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class CepRequestDto
    {
        public CepRequestDto() { }
        public string latitude { get; set; } = string.Empty;
        public string longitude { get; set; } = string.Empty;
    }
}
