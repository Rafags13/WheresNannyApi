using Arch.EntityFrameworkCore.UnitOfWork;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
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
        private readonly IHttpClientFactory _httpClientFactory;
        public PersonService(IRepository repository, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory) 
        { 
            _repository = repository;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
        }
        #region Common User Home Information

        public async Task<UserHomeInformationDto> GetUserMainPageInformation(FindCommonUserServicesDto findCommonUserServicesDto)
        {
            var servicesReference =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList(
                    include: x =>
                        x.Include(x => x.NannyService)
                        .ThenInclude(x => x.Person)
                        )
                .Items
                .Where(x => x.PersonId == findCommonUserServicesDto.PersonId);
            
            var nannyListOrderedByNearCep = NannyListOrderedByFilter(
                new ChangeNannyListByFilterDto {
                    Filter = "location",
                    Cep = findCommonUserServicesDto.Cep
                });

            var mostRecentService = servicesReference.Count() > 0 ? servicesReference.OrderByDescending(x => x.HiringDate).First() : null;
            var recentCardDto = new RecentCardDto
            {
                PersonName = mostRecentService.NannyService.Person.Fullname,
                ImageUri = mostRecentService.NannyService.Person.ImageUri,
                ServiceId = mostRecentService.Id,
                Date = mostRecentService.HiringDate
            };

            var data = new UserHomeInformationDto
            {
                NannyListOrderedByFilter = nannyListOrderedByNearCep,
                MostRecentService = recentCardDto,
            };

            return data;
        }

        public List<NannyCardDto> NannyListOrderedByFilter(ChangeNannyListByFilterDto changeNannyListByFilterDto)
        {
            var nannysReference = _unitOfWork.GetRepository<Nanny>()
                .GetPagedList(include: x => x
                    .Include(x => x.Person)
                        .ThenInclude(x => x.Address)
                    .Include(x => x.Person)
                    .Include(x => x.CommentsRankNanny)
                    ).Items;

            var nannysReferenceOrderedByFilter = changeNannyListByFilterDto.Filter switch
            {
                "location" => NannyListOrderedByNearCep(nannysReference, changeNannyListByFilterDto.Cep),
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

        public NannyContractDto GetNannyInfoToContractById(int id, int userId)
        {
            var currentPerson =
                _unitOfWork.GetRepository<Person>()
                .GetPagedList(
                    include: x =>
                        x.Include(x => x.Address)
                    )
                .Items
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
            var nanny =
                _unitOfWork.GetRepository<Nanny>()
                .GetPagedList(
                    include: x =>
                        x.Include(x => x.Person)
                        .Include(x => x.Person)
                            .ThenInclude(x => x.Address)
                        .Include(x => x.CommentsRankNanny))
                .Items
                .Where(x => x.Id == id)
                .FirstOrDefault();

            GeoCoordinate currentPersonCoordinate = new GeoCoordinate(currentPerson.Address.Latitude ?? 0.0, currentPerson.Address.Longitude ?? 0.0);
            GeoCoordinate nannyPersonCoordinate = new GeoCoordinate(nanny.Person.Address.Latitude ?? 0.0, nanny.Person.Address.Longitude ?? 0.0);

            double distanceBetweenPersonAndNanny = currentPersonCoordinate.GetDistanceTo(nannyPersonCoordinate);

            var nannyContractInformation = new NannyContractDto
            {
                NannyId = nanny.Id,
                ImageProfileBase64Uri = nanny.Person.ImageUri,
                RankAverageStars = nanny.RankAvegerageStars,
                RankCommentCount = nanny.RankCommentCount,
                ServicePrice = nanny.ServicePrice,
                Address = new NannyAddressPersonContractDto { Cep = nanny.Person.Address.Cep, HouseNumber = nanny.Person.Address.HouseNumber, DistanceBetweenThePeople = Convert.ToInt32(distanceBetweenPersonAndNanny).ToString() },
                Person = new NannyPersonContractDto { Cellphone = nanny.Person.Cellphone, Email = nanny.Person.Email, Name = nanny.Person.Fullname }
            };

            return nannyContractInformation;
        }

        
    }
}
