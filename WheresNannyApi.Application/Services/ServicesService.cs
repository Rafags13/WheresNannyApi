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
                _unitOfWork.GetRepository<User>()
                .GetPagedList(
                    include: x =>
                        x.Include(x => x.Person)
                            .ThenInclude(x => x.Nanny))
                .Items
                .Where(x => x.Person?.Nanny?.Id == createContractNannyDto.NannyId)
                .Select(x => x.DeviceId)
                .FirstOrDefault();

            var nameFromPersonWhoHire = _unitOfWork.GetRepository<Person>().GetPagedList().Items.Where(x => x.Id == createContractNannyDto.PersonId).Select(x => x.Fullname).FirstOrDefault();

            await _repository.AddAsync(newService);
            await _repository.SaveChangesAsync();

            var currentService =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList()
                .Items
                .LastOrDefault();

            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    {"message", $"Um novo serviço de {nameFromPersonWhoHire} foi chamado, deseja aceitar?" },
                    {"serviceId", $"{currentService.Id}" }
                },
                Token = mobileNannyDeviceId,
                Notification = new Notification()
                {
                    Title = "Novo serviço",
                    Body = $"Um novo serviço de {nameFromPersonWhoHire} foi chamado, deseja aceitar?",
                },
            };

            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

            return true;
        }

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

        public void ServiceAccepted(AcceptedServiceDto acceptedServiceDto)
        {
            var service = _unitOfWork.GetRepository<Service>().GetPagedList(include: x => x.Include(x => x.NannyService).ThenInclude(x => x.Person).Include(x => x.PersonService).ThenInclude(x => x.User)).Items.Where(x => x.Id == acceptedServiceDto.ServiceId).FirstOrDefault();

            if(service != null)
            {
                service.ServiceAccepted = acceptedServiceDto.Accepted;
                _unitOfWork.GetRepository<Service>().Update(service);
                _unitOfWork.SaveChanges();
            }

            var acceptedServiceMessage = acceptedServiceDto.Accepted ? "Foi aceito" : "Foi recusado";
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    {"message", $"O serviço da babá {service?.PersonService?.Fullname} {acceptedServiceMessage}" },
                    {"accepted", acceptedServiceDto.Accepted ? "true" : "false" }
                },
                Token = service?.PersonService?.User?.DeviceId,
                Notification = new Notification()
                {
                    Title = "Novo serviço",
                    Body = $"O serviço da babá {service?.PersonService?.Fullname} {acceptedServiceMessage}",
                },
            };

            _firebaseMessagerService.SendNotification(message);
        }
    }
}
