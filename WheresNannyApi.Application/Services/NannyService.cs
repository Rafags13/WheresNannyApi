using Arch.EntityFrameworkCore.UnitOfWork;
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
    public class NannyService : INannyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository _repository;
        public NannyService(IUnitOfWork unitOfWork, IRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<List<ServiceNannyCardDto>> GetAllNannyServices(int userId)
        {
            var currentNannyId =
                _unitOfWork.GetRepository<Nanny>()
                .GetPagedList(include: x =>
                    x.Include(x => x.Person)
                    .Include(x => x.ServicesNanny))
                .Items
                .Where(x => x.Person.UserId == userId)
                .First().Id;

            var currentServicesFromNanny =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList(
                    orderBy: x => x.OrderByDescending(x => x.HiringDate),
                    include: x =>
                    x.Include(x => x.PersonService),
                    pageIndex: 0,
                    pageSize: 10
                ).Items
                .Select(x => new ServiceNannyCardDto {
                    ServiceId = x.Id,
                    HiringDate = x.HiringDate,
                    ClientName = x.PersonService.Fullname,
                    ServicePrice = x.Price
                }).ToList();


            return currentServicesFromNanny;
        }
    }
}
