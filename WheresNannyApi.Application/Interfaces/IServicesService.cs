﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Interfaces
{
    public interface IServicesService
    {
        public Task<bool> CreateService(CreateContractNannyDto createContractNannyDto);
        public List<RecentCardDto> ListAllServices(int userId, int pageIndex);
        public NannyServiceInformationDto GetNannyServiceInformation(int serviceId);
        public ServiceInformationDto GetServiceInformations(int serviceId, bool isNanny);
        public void ServiceAccepted(AcceptedServiceDto acceptedServiceDto);
        public Task<string> CancelTheService(int serviceId, bool isClient);
    }
}
