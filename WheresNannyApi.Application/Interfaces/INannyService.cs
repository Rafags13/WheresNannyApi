using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Interfaces
{
    public interface INannyService
    {
        public Task<List<ServiceNannyCardDto>> GetAllNannyServices(int userId, int pageIndex);
        public NannyDashboardInformationDto GetNannyDashboardInformationDto(int userId);
    }
}
