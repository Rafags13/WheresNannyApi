using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class UserHomeInformationDto
    {
        public UserHomeInformationDto() { }
        public List<NannyCardDto>? NannyListOrderedByFilter { get; set; }
        public RecentCardDto? MostRecentService { get; set; }

    }
}
