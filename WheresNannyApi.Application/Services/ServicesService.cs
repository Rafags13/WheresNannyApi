using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWork _unitOfWork;
        public ServicesService(IRepository repository, IUnitOfWork unitOfWork) 
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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

        public List<RecentCardDto> ListAllServices(int userId, int pageIndex)
        {
            var allServicesFromUser =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList(include: x =>
                    x.Include(x => x.NannyService)
                        .ThenInclude(x => x.Person)
                    .Include(x => x.PersonService)
                , pageIndex: pageIndex, pageSize: 10).Items
                .Where(x => x.PersonId == userId)
                .Select(x => new RecentCardDto { 
                    PersonName = x.NannyService.Person.Fullname,
                    ImageUri = x.NannyService.Person.ImageUri,
                    ServiceId = x.Id,
                    Date = x.HiringDate
                }).OrderByDescending(x => x.Date).ToList();

            return allServicesFromUser;
        }
    }
}
