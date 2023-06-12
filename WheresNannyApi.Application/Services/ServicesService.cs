using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IRepository _repository;
        public ServicesService(IRepository repository) 
        {
            _repository = repository;
        }
        public async Task<bool> CreateService(CreateContractNannyDto createContractNannyDto)
        {
            var newService = new Service(
                createContractNannyDto.ServiceFinishHour.TimeOfDay,
                createContractNannyDto.HiringDate,
                createContractNannyDto.Price,
                createContractNannyDto.PersonId,
                createContractNannyDto.NannyId
                );

            await _repository.AddAsync( newService );
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
