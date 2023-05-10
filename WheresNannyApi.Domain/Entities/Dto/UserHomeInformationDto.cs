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
        public List<Service>? ServicesFilteredByPerson { get; set; }
        public List<Nanny>? NannysListOrderedByRankStarts { get; set; }
        public Service? MostRecentService { get; set; }

    }
}
