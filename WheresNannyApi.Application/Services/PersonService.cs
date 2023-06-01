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
        #region Common User Home Information

        public async Task<UserHomeInformationDto> GetUserMainPageInformation(FindCommonUserServicesDto findCommonUserServicesDto)
        {
            var servicesReference = await _repository.GetListAsync<Service>();
            var servicesFilteredByPerson = servicesReference.Where(x => x.PersonId == findCommonUserServicesDto.PersonId);

            var nannyListOrderedByNearCep = NannyListOrderedByFilter("location", findCommonUserServicesDto.Cep);

            var mostRecentService = servicesFilteredByPerson.Count() > 0 ? servicesFilteredByPerson.OrderByDescending(x => x.HiringDate).First() : null;

            var data = new UserHomeInformationDto
            {
                NannyListOrderedByFilter = nannyListOrderedByNearCep,
                MostRecentService = mostRecentService,
            };

            return data;
        }

        private List<NannyCardDto> NannyListOrderedByFilter(string filter, string cep = "")
        {
            var nannysReference = _unitOfWork.GetRepository<Nanny>()
                .GetPagedList(include: x => x
                    .Include(x => x.Person)
                        .ThenInclude(x => x.Address)
                    .Include(x => x.Person)
                    .Include(x => x.CommentsRankNanny)
                    ).Items;

            var nannysReferenceOrderedByFilter = filter switch
            {
                "location" => NannyListOrderedByNearCep(nannysReference, cep),
                "price" => NannyListOrderedByPrice(nannysReference),
                "rank" => NannyListOrderedByRank(nannysReference),
                _ => nannysReference.ToList()
            };

            var listNannyCardObject = CreateModelNannyCardObject(nannysReferenceOrderedByFilter);

            return listNannyCardObject;
        }

        private List<Nanny> NannyListOrderedByNearCep(IList<Nanny>? nannies, string cep)
        {
            if (nannies is null) return new List<Nanny>();

            nannies.Select(x => x.Person?.Address?.Cep).ToList().Sort((a, b) => Math.Abs(int.Parse(cep) - int.Parse(a)) - Math.Abs(int.Parse(cep) - int.Parse(b)));

            return nannies.ToList();
        }

        private List<Nanny> NannyListOrderedByRank(IList<Nanny>? nannies)
        {
            if (nannies is null) return new List<Nanny>();

            var nannyOrdered = nannies.OrderByDescending(x => x.RankAvegerageStars).ToList();

            return nannyOrdered;

        }

        private List<Nanny> NannyListOrderedByPrice(IList<Nanny>? nannies)
        {
            if (nannies is null) return new List<Nanny>();

            var nannyOrdered = nannies.OrderBy(x => x.ServicePrice).ToList();

            return nannyOrdered;
        }

        private List<NannyCardDto> CreateModelNannyCardObject(List<Nanny> nannysReference)
        {
            var nannyCardDtoList = new List<NannyCardDto>();

            foreach (var nanny in nannysReference)
            {
                nannyCardDtoList.Add(new NannyCardDto
                {
                    Id = nanny.Id,
                    Fullname = nanny.Person.Fullname,
                    StarsCounting = nanny.RankAvegerageStars,
                    RankCommentCount = nanny.RankCommentCount,
                    ImageUri = nanny.Person.ImageUri
                });
            }

            return nannyCardDtoList;
        }
        #endregion


    }
}
