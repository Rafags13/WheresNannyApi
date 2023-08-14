using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Application.Util;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseMessagerService _firebaseMessagerService;
        public ServicesService(IRepository repository, IUnitOfWork unitOfWork, IFirebaseMessagerService firebaseMessagerService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _firebaseMessagerService = firebaseMessagerService;
        }

        #region Create
        public async Task<bool> CreateService(CreateContractNannyDto createContractNannyDto)
        {
            var newService = new Service(
                createContractNannyDto.ServiceFinishHour.TimeOfDay,
                createContractNannyDto.HiringDate,
                createContractNannyDto.Price,
                createContractNannyDto.PersonId,
                createContractNannyDto.NannyId
                );

            var mobileNannyDeviceId = 
                _unitOfWork.GetRepository<User>().GetFirstOrDefault(
                    include: x =>
                        x.Include(x => x.Person)
                            .ThenInclude(x => x.Nanny),
                   predicate: x => x.Person.Nanny.Id == createContractNannyDto.NannyId)
                .DeviceId;

            if (string.IsNullOrWhiteSpace(mobileNannyDeviceId)) throw new Exception("A babá não está disponível no momento, tente novamente.");

            await _repository.AddAsync(newService);
            await _repository.SaveChangesAsync();

            var currentService =
                _unitOfWork.GetRepository<Service>()
                .GetFirstOrDefault(
                    orderBy: x => x.OrderByDescending(x => x.Id));
            var response = new { serviceId = currentService.Id, expireServiceDate = DateTime.Now.AddMinutes(5)};
            
            var nameFromPersonWhoHire = _unitOfWork.GetRepository<Person>().GetFirstOrDefault(predicate: x => x.Id == createContractNannyDto.PersonId).Fullname;
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    {"message", $"Um novo serviço foi chamado, deseja aceitar?" },
                    {"response", Newtonsoft.Json.JsonConvert.SerializeObject(response)}
                },
                Token = mobileNannyDeviceId,
                Notification = new Notification()
                {
                    Title = "Novo serviço",
                    Body = $"Um novo serviço foi chamado, deseja aceitar?",
                },
            };

            _ = _firebaseMessagerService.SendNotification(message);

            return true;
        }
        #endregion

        #region List All Services
        public List<RecentCardDto> ListAllServices(int userId, int pageIndex)
        {
            var allServicesFromUser =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList(orderBy: x => x.OrderByDescending(x => x.HiringDate), include: x =>
                    x.Include(x => x.NannyService)
                        .ThenInclude(x => x.Person)
                    .Include(x => x.PersonService)
                , pageIndex: pageIndex, pageSize: 10).Items
                .Where(x => x.PersonId == userId)
                .Select(x => new RecentCardDto
                {
                    PersonName = x.NannyService.Person.Fullname,
                    ImageUri = x.NannyService.Person.ImageUri,
                    ServiceId = x.Id,
                    Date = x.HiringDate
                }).OrderByDescending(x => x.Date).ToList();

            return allServicesFromUser;
        }
        #endregion

        #region Nanny Service Information
        public NannyServiceInformationDto GetNannyServiceInformation(int serviceId)
        {
            var currentService =
                _unitOfWork.GetRepository<Service>()
                .GetFirstOrDefault(
                    predicate: x => x.Id == serviceId,
                    include: x => 
                        x.Include(x => x.NannyService)
                            .ThenInclude(x => x.Person)
                        .Include(x => x.NannyService)
                            .ThenInclude(x => x.CommentsRankNanny)
                        .Include(x => x.PersonService)
                            .ThenInclude(x => x.Address)
                    );

            NannyServiceInformationDto serviceInformation = 
                new()
                {
                    ParentName = currentService?.PersonService?.Fullname,
                    NannyName = currentService?.NannyService.Person.Fullname,
                    Cep = currentService?.PersonService.Address.Cep,
                    HiringDate = currentService.HiringDate,
                    NannyCountStars = currentService.NannyService.RankAvegerageStars,
                    NannyId = currentService.NannyService.Id,
                    ServicePrice = currentService.Price
                };
            
            return serviceInformation;
        }
        #endregion

        #region Accept Service
        public void ServiceAccepted(AcceptedServiceDto acceptedServiceDto)
        {
            var currentService = 
                _unitOfWork.GetRepository<Service>()
                .GetFirstOrDefault(
                    include: x => 
                        x.Include(x => x.NannyService)
                            .ThenInclude(x => x.Person)
                        .Include(x => x.PersonService)
                            .ThenInclude(x => x.User),
                    predicate: x => x.Id == acceptedServiceDto.ServiceId);

            if(currentService != null)
            {
                currentService.ServiceAccepted = acceptedServiceDto.Accepted;
                _unitOfWork.GetRepository<Service>().Update(currentService);
                _unitOfWork.SaveChanges();
            }

            var acceptedServiceMessage = acceptedServiceDto.Accepted ? "Foi aceito" : "Foi recusado";

            object response = new { };

            if(acceptedServiceDto.Accepted)
            {
                response = new
                {
                    accepted = acceptedServiceDto.Accepted,
                    serviceId = acceptedServiceDto.ServiceId.ToString(),
                    chatPersonName = currentService.NannyService.Person.Fullname
                };
            } else
            {
                response = new
                {
                    accepted = acceptedServiceDto.Accepted
                };
            }

            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    {"message", $"O serviço da babá {currentService?.NannyService.Person.Fullname} {acceptedServiceMessage}" },
                    {"response", Newtonsoft.Json.JsonConvert.SerializeObject(response)}
                },
                Token = currentService?.PersonService?.User?.DeviceId,
                Notification = new Notification()
                {
                    Title = "Novo serviço",
                    Body = $"O serviço da babá {currentService?.NannyService.Person.Fullname} {acceptedServiceMessage}",
                },
            };

            _firebaseMessagerService.SendNotification(message);
        }
        #endregion

        #region Display Service From Parent
        #endregion

        #region Display Service From Nanny
        public ServiceNannyInformationDto GetServiceInformationsFromNanny(int serviceId)
        {
            var serviceInformations =
                _unitOfWork.GetRepository<Service>()
                .GetFirstOrDefault(
                    predicate: x => x.Id == serviceId,
                    include: x =>
                        x.Include(x => x.PersonService)
                            .ThenInclude(x => x.Address)
                         .Include(x => x.PersonService)
                         .Include(x => x.NannyService)
                         .Include(x => x.NannyService)
                            .ThenInclude(x => x.Person)
                                .ThenInclude(x => x.Address))
                ?? throw new Exception("Não foi possível encontrar o serviço. Contate um administrador.");

            var firstCoordinate = new CoordinateDto 
            {
                Latitude = serviceInformations.PersonService.Address.Latitude ?? 0.0f,
                Longitude = serviceInformations.PersonService.Address.Longitude ?? 0.0f,
            };

            var secondCoordinate = new CoordinateDto
            {
                Latitude = serviceInformations.NannyService.Person.Address.Latitude ?? 0.0f,
                Longitude = serviceInformations.NannyService.Person.Address.Longitude ?? 0.0f,
            };

            var distance = Functions.DistanceBetweenTwoPoints(firstCoordinate, secondCoordinate);

            var data = new ServiceNannyInformationDto
            {
                ParentName = serviceInformations.PersonService.Fullname,
                ParentEmail = serviceInformations.PersonService.Email,
                ParentCellphone = serviceInformations.PersonService.Cellphone,
                ParentBirthdayDate = serviceInformations.PersonService.BirthdayDate,
                ParentPictureBase64 = serviceInformations.PersonService.ImageUri,
                ParentCep = serviceInformations.PersonService.Address.Cep,
                ServiceFinishHour = serviceInformations.ServiceFinishHour,
                ServicePrice = serviceInformations.Price,
                Distance = distance,
                OriginCoordinates = new CoordinateDto
                {
                    Latitude = serviceInformations.PersonService.Address.Latitude ?? 0.0f,
                    Longitude = serviceInformations.PersonService.Address.Longitude ?? 0.0f
                },
                DestinationCoordinates = new CoordinateDto
                {
                    Latitude = serviceInformations.NannyService.Person.Address.Latitude ?? 0.0f,
                    Longitude = serviceInformations.NannyService.Person.Address.Longitude ?? 0.0f
                }
            };

            return data;
        }
        #endregion
    }
}
