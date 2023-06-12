using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Interfaces
{
    public interface IPersonService
    {
        Task<UserHomeInformationDto> GetUserMainPageInformation(FindCommonUserServicesDto findCommonUserServicesDto);
        List<NannyCardDto> NannyListOrderedByFilter(ChangeNannyListByFilterDto changeNannyListByFilterDto);
        NannyContractDto GetNannyInfoToContractById(int id, int userId);
    }
}
