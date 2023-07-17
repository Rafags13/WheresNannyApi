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
        public Task<UserHomeInformationDto> GetUserMainPageInformation(FindCommonUserServicesDto findCommonUserServicesDto);
        public List<NannyCardDto> NannyListOrderedByFilter(ChangeNannyListByFilterDto changeNannyListByFilterDto);
        public NannyContractDto GetNannyInfoToContractById(int id, int userId);
        public UpdateProfileInformationDto? ProfileListInformation(int userId);
        public Task<string> UpdateProfileInformation (UpdateProfileInformationDto updateProfileInformationDto);
    }
}
