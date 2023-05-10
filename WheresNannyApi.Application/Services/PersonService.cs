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
        public PersonService(IRepository repository) 
        { 
            _repository = repository;
        }

        public async Task<UserHomeInformationDto> GetUserMainPageInformation(int personId)
        {
            var servicesReference = await _repository.GetListAsync<Service>();
            var servicesFilteredByPerson = servicesReference.Where(x => x.PersonId == personId).ToList();

            var nannysReference = await _repository.GetListAsync<Nanny>();
            var nannysListOrderedByRankStars = nannysReference.OrderBy(x => x.RankAvegerageStars).Take(2).ToList();

            var mostRecentService = servicesFilteredByPerson.OrderByDescending(x => x.HiringDate).First();

            var data = new UserHomeInformationDto
            {
                ServicesFilteredByPerson = servicesFilteredByPerson,
                NannysListOrderedByRankStarts = nannysListOrderedByRankStars,
                MostRecentService = mostRecentService,
            };

            return data;

        }
    }
}
