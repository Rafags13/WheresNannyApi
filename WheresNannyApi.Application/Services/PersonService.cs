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

            var nannysListOrderedByNearCep = NannyListOrderedByNearCep(findCommonUserServicesDto.Cep);

            var mostRecentService = servicesFilteredByPerson.Count() > 0 ? servicesFilteredByPerson.OrderByDescending(x => x.HiringDate).First() : null;

            var data = new UserHomeInformationDto
            {
                NannyListOrderedByFilter = nannysListOrderedByNearCep,
                MostRecentService = mostRecentService,
            };

            return data;

        }

        private List<NannyCardDto> NannyListOrderedByNearCep(string cep)
        {
            var nannysReference = _unitOfWork.GetRepository<Nanny>()
                .GetPagedList(include: x => x
                    .Include(x => x.Person)
                        .ThenInclude(x => x.Address)
                    .Include(x => x.Person)
                    .Include(x => x.CommentsRankNanny)
                    ).Items;

            nannysReference.Select(x => x.Person?.Address?.Cep).ToList().Sort((a, b) => Math.Abs(int.Parse(cep) - int.Parse(a)) - Math.Abs(int.Parse(cep) - int.Parse(b)));

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
