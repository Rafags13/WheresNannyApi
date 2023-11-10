using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
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

        #region Get All Services
        public List<ServiceNannyCardDto> GetAllNannyServices(int userId, int pageIndex)
        {
            var currentNannyId = 
                _unitOfWork.GetRepository<Nanny>()
                .GetFirstOrDefault(include: x =>
                    x.Include(x => x.Person)
                    .Include(x => x.ServicesNanny), 
                        predicate: x => x.Person!.UserId == userId).Id;

            var currentServicesFromNanny =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList(
                    orderBy: x => x.OrderByDescending(x => x.HiringDate),
                    include: x =>
                    x.Include(x => x.PersonService),
                    pageIndex: pageIndex,
                    pageSize: 10
                ).Items.Where(x => x.NannyId == currentNannyId)
                .Select(x => new ServiceNannyCardDto
                {
                    ServiceId = x.Id,
                    HiringDate = x.HiringDate,
                    ClientName = x.PersonService!.Fullname,
                    ServicePrice = x.Price
                }).ToList();


            return currentServicesFromNanny;
        }

        #endregion

        #region Nanny Dashboard Information
        public NannyDashboardInformationDto GetNannyDashboardInformationDto(int userId)
        {

            var currentNanny = _unitOfWork.GetRepository<Nanny>().GetFirstOrDefault(include: x =>
                    x.Include(x => x.Person)
                    .Include(x => x.ServicesNanny), predicate: x => x.Person!.UserId == userId);

            var lastServiceFromNanny =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList(predicate: x => x.NannyId == currentNanny.Id,
                include:x =>
                    x.Include(x => x.PersonService))
                .Items
                .Select(x => new ServiceNannyCardDto { ClientName = x.PersonService!.Fullname, ServiceId = x.Id, HiringDate = x.HiringDate, ServicePrice = x.Price })
                .LastOrDefault();

            var countingFromServiceListOfThisNanny = _unitOfWork.GetRepository<Service>().Count(x => x.NannyId == currentNanny.Id);

            var serviceList = _unitOfWork.GetRepository<Service>().GetPagedList(predicate: x => x.NannyId == currentNanny.Id, pageSize: countingFromServiceListOfThisNanny).Items;

            DateTime lastSixMonthsBefore = DateTime.Now.AddMonths(-5);
            List<CountingChartDto> countingChartData = new List<CountingChartDto>();
            List<string> monthNames = new List<string>();
            List<EarnCountingChartDto> earnChartData = new List<EarnCountingChartDto>();

            while (lastSixMonthsBefore <= DateTime.Now)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(lastSixMonthsBefore.Month).ToUpper();
                var countingService = serviceList.Where(x => x.HiringDate.Month == lastSixMonthsBefore.Month).Count();
                monthNames.Add(monthName);
                countingChartData.Add(new CountingChartDto(countingService, lastSixMonthsBefore.Month));
                earnChartData.Add(new EarnCountingChartDto(currentNanny.ServicePrice * countingService));
                lastSixMonthsBefore = lastSixMonthsBefore.AddMonths(1);
            }

            var returnData = new NannyDashboardInformationDto {
                LastService = lastServiceFromNanny,
                CountingServiceChart = countingChartData,
                EarnCountingChart = earnChartData,
                MonthNames = monthNames
            };

            return returnData;
        }
        #endregion

        #region Calculate earn by month

        public EarnFromNannyByMonthDto GetEarnsByMonth(int month, int userId)
        {
            var nannyId = _unitOfWork.GetRepository<Nanny>().GetFirstOrDefault(predicate: x => x.Person.UserId == userId, include: x => x.Include(x => x.Person)).Id;
            var serviceRepository = _unitOfWork.GetRepository<Service>();
            
            var countingServicesFromThatNanny = serviceRepository.Count(x => x.NannyId == nannyId && x.HiringDate.Month == month);

            var allServicesFromThatNannyByMonth =
                serviceRepository
                .GetPagedList(
                    predicate: x => x.NannyId == nannyId && x.HiringDate.Month == month,
                    include: x =>
                        x.Include(x => x.NannyService)
                        .ThenInclude(x => x.Person)
                        .Include(x => x.PersonService),
                    pageSize: countingServicesFromThatNanny).Items.Select(x => new { ServiceId = x.Id, Price = x.Price, PersonWhoHire = new { Id = x.PersonId, PersonFullname = x.PersonService.Fullname, DateFromHire = x.HiringDate, UrlPhoto = x.PersonService.ImageUri } });

            var totalEarn = allServicesFromThatNannyByMonth.Sum(x => x.Price);

            var personsWhoPayed = allServicesFromThatNannyByMonth.GroupBy(x => x.PersonWhoHire.Id).Select(x => new MainPayer { Id = x.Key, FirstServiceId=x.FirstOrDefault().ServiceId, Name = x.FirstOrDefault().PersonWhoHire.PersonFullname, UriClient = x.FirstOrDefault().PersonWhoHire.UrlPhoto,TotalPayment = x.Sum(x => x.Price), DateFromFirstHire = x.FirstOrDefault().PersonWhoHire.DateFromHire}).OrderBy(x => x.TotalPayment).Take(3);

            return new EarnFromNannyByMonthDto { TotalEarn = totalEarn, MainPeopleWhoHireHer = personsWhoPayed };
        }

        #endregion
    }
}
