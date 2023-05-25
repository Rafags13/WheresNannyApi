using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public PersonService(IRepository repository, IUnitOfWork unitOfWork) 
        { 
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserHomeInformationDto> GetUserMainPageInformation(FindCommonUserServicesDto findCommonUserServicesDto)
        {
            var servicesReference = await _repository.GetListAsync<Service>();
            var servicesFilteredByPerson = servicesReference.Where(x => x.PersonId == findCommonUserServicesDto.PersonId);

            var nannysReference = await _repository.GetListAsync<Nanny>();
            var nannysListOrderedByNearCep = NannyListOrderedByNearCep(findCommonUserServicesDto.Cep);

            var mostRecentService = servicesFilteredByPerson.Count() > 0 ? servicesFilteredByPerson.OrderByDescending(x => x.HiringDate).First() : null;

            var data = new UserHomeInformationDto
            {
                NannyListOrderedByFilter = nannysListOrderedByNearCep,
                MostRecentService = mostRecentService,
            };

            return data;

        }

        private List<Nanny> NannyListOrderedByNearCep(string cep)
        {
            var nannysReference = _unitOfWork.GetRepository<Nanny>()
                .GetPagedList(include: x => x
                    .Include(x => x.Person)
                        .ThenInclude(x => x.Address)
                    .Include(x => x.Person)).Items;

            nannysReference.Select(x => x.Person?.Address?.Cep).ToList().Sort((a, b) => Math.Abs(int.Parse(cep) - int.Parse(a)) - Math.Abs(int.Parse(cep) - int.Parse(b)));

            return nannysReference.ToList();
        }
    }
}
