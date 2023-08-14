using Arch.EntityFrameworkCore.UnitOfWork;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Application.Util;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAddressService _addressService;
        public PersonService(IRepository repository, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IAddressService addressService) 
        { 
            _repository = repository;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _addressService = addressService;
        }

        #region Common User Home Information

        public async Task<UserHomeInformationDto> GetUserMainPageInformation(FindCommonUserServicesDto findCommonUserServicesDto)
        {
            var nannyListOrderedByNearCep = NannyListOrderedByFilter(
                new ChangeNannyListByFilterDto {
                    Filter = "location",
                    Cep = findCommonUserServicesDto.Cep
                });

            var mostRecentService = 
                _unitOfWork.GetRepository<Service>()
                .GetFirstOrDefault(
                    include: x =>
                        x.Include(x => x.NannyService)
                            .ThenInclude(x => x.Person),
                    predicate: x => x.PersonId == findCommonUserServicesDto.PersonId);
            
            var recentCardDto = mostRecentService is not null ?
                new RecentCardDto
            {
                PersonName = mostRecentService.NannyService.Person.Fullname,
                ImageUri = mostRecentService.NannyService.Person.ImageUri,
                ServiceId = mostRecentService.Id,
                Date = mostRecentService.HiringDate
            } : null;

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

            var listNannyCardObject = CreateModelNannyCardObject(nannysReferenceOrderedByFilter.Take(3).ToList());

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

        #region Nanny Contract Information

        public NannyContractDto GetNannyInfoToContractById(int id, int userId)
        {
            var currentPerson = _unitOfWork.GetRepository<Person>().GetFirstOrDefault(include: x => x.Include(x => x.Address), predicate: x => x.UserId == userId);
            var currentNanny = _unitOfWork.GetRepository<Nanny>().GetFirstOrDefault(include: x =>
                        x.Include(x => x.Person)
                        .Include(x => x.Person)
                            .ThenInclude(x => x.Address)
                        .Include(x => x.CommentsRankNanny), predicate: x => x.Id == id);

            var firstCoordinate = new CoordinateDto
            {
                Latitude = currentPerson.Address.Latitude ?? 0.0f,
                Longitude = currentPerson.Address.Longitude ?? 0.0f 
            };

            var secondCoordinate = new CoordinateDto
            {
                Latitude = currentNanny.Person.Address.Latitude ?? 0.0f,
                Longitude = currentNanny.Person.Address.Longitude ?? 0.0f
            };

            double distanceBetweenPersonAndNanny = Functions.DistanceBetweenTwoPoints(firstCoordinate, secondCoordinate);

            var nannyContractInformation = new NannyContractDto
            {
                NannyId = currentNanny.Id,
                ImageProfileBase64Uri = currentNanny.Person.ImageUri,
                RankAverageStars = currentNanny.RankAvegerageStars,
                RankCommentCount = currentNanny.RankCommentCount,
                ServicePrice = currentNanny.ServicePrice,
                Address = new NannyAddressPersonContractDto { Cep = currentNanny.Person.Address.Cep, HouseNumber = currentNanny.Person.Address.HouseNumber, DistanceBetweenThePeople = Convert.ToInt32(distanceBetweenPersonAndNanny).ToString() },
                Person = new NannyPersonContractDto { Cellphone = currentNanny.Person.Cellphone, Email = currentNanny.Person.Email, Name = currentNanny.Person.Fullname }
            };

            return nannyContractInformation;
        }
        #endregion

        #region Display Profile Information
        public UpdateProfileInformationDto? ProfileListInformation(int userId)
        {
            var currentPerson = _unitOfWork.GetRepository<Person>().GetFirstOrDefault(include: x => x.Include(x => x.Address), predicate: x => x.UserId == userId);
            if (currentPerson == null) return null;

            var updateProfileDto = new UpdateProfileInformationDto
            {
                AddressFromUpdateInformation = new AddressFromUpdateInformationDto
                {
                    Cep = currentPerson.Address.Cep,
                    Complement = currentPerson.Address.Complement ?? "",
                    Number = currentPerson.Address.HouseNumber ?? ""
                },
                PersonInformation = new PersonInformationDto
                {
                    Cpf = currentPerson.Cpf,
                    Fullname = currentPerson.Fullname,
                    Cellphone = currentPerson.Cellphone,
                    Email = currentPerson.Email,
                    ImageBase64 = currentPerson.ImageUri
                }
            };

            return updateProfileDto;
        }
        #endregion

        #region Update Profile Information
        public async Task<string> UpdateProfileInformation(UpdateProfileInformationDto updateProfileInformationDto)
        {
            var errorMessage = ReturnMessageIfUserCantBeUpdated(updateProfileInformationDto);
            if(errorMessage != "") return errorMessage;

            var currentUser = 
                _unitOfWork.GetRepository<Person>()
                .GetFirstOrDefault(
                    include: x => x.Include(x => x.Address),
                    predicate: x => x.UserId == updateProfileInformationDto.PersonInformation.Id);

            currentUser.Fullname = updateProfileInformationDto.PersonInformation.Fullname;
            currentUser.Cpf = updateProfileInformationDto.PersonInformation.Cpf;
            currentUser.Email = updateProfileInformationDto.PersonInformation.Email;
            currentUser.Cellphone = updateProfileInformationDto.PersonInformation.Cellphone;

            var currentAddress = _unitOfWork.GetRepository<Address>().GetFirstOrDefault(predicate: x => x.Cep == updateProfileInformationDto.AddressFromUpdateInformation.Cep);

            var newAddressDontExistsYet = currentAddress == null;

            if (newAddressDontExistsYet)
            {
                _ = _addressService.CreateAddress(
                    new CreateAddressDto
                    {
                        Cep = updateProfileInformationDto.AddressFromUpdateInformation.Cep,
                        Complement = updateProfileInformationDto.AddressFromUpdateInformation.Complement,
                        Number = updateProfileInformationDto.AddressFromUpdateInformation.Number
                    });

                var addressAfterAddedInSystem = _unitOfWork.GetRepository<Address>().GetFirstOrDefault(predicate: x => x.Cep == updateProfileInformationDto.AddressFromUpdateInformation.Cep);

                currentUser.AddressId = addressAfterAddedInSystem.Id;
            }
            else
            {
                currentUser.AddressId = currentAddress.Id;
            }

            _repository.Update(currentUser);
            await _repository.SaveChangesAsync();

            return "";
        }

        private string ReturnMessageIfUserCantBeUpdated(UpdateProfileInformationDto updateProfileInformationDto)
        {
            var personContext = _unitOfWork.GetRepository<Person>();

            var cpfAlreadyExistsInSystem = personContext.GetFirstOrDefault(predicate: x => x.Cpf == updateProfileInformationDto.PersonInformation.Cpf && updateProfileInformationDto.PersonInformation.Id != x.UserId);
            if (cpfAlreadyExistsInSystem != null) return "Já existe uma pessoa cadastrada com esse CPF no sistema. Tente Novamente.";

            var emailAlreadyExistsInSystem = personContext.GetFirstOrDefault(predicate: x => x.Email == updateProfileInformationDto.PersonInformation.Email && updateProfileInformationDto.PersonInformation.Id != x.UserId);
            if (emailAlreadyExistsInSystem != null) return "Já existe uma pessoa cadastrada com esse E-mail no sistema. Tente Novamente.";

            return "";
        }
        #endregion
    }
}
