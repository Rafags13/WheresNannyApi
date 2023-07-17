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
        public async Task<List<ServiceNannyCardDto>> GetAllNannyServices(int userId, int pageIndex)
        {
            var currentNannyId = _unitOfWork.GetRepository<Nanny>().GetFirstOrDefault(include: x =>
                    x.Include(x => x.Person)
                    .Include(x => x.ServicesNanny), predicate: x => x.Person.UserId == userId).Id;

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
                    ClientName = x.PersonService.Fullname,
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
                    .Include(x => x.ServicesNanny), predicate: x => x.Person.UserId == userId);

            var lastServiceFromNanny =
                _unitOfWork.GetRepository<Service>()
                .GetPagedList(include:x =>
                    x.Include(x => x.PersonService))
                .Items
                .Where(x => x.NannyId == currentNanny.Id)
                .Select(x => new ServiceNannyCardDto { ClientName = x.PersonService.Fullname, ServiceId = x.Id, HiringDate = x.HiringDate, ServicePrice = x.Price })
                .LastOrDefault();

            var serviceList = _unitOfWork.GetRepository<Service>().GetPagedList().Items.Where(x => x.NannyId == currentNanny.Id);

            DateTime lastSixMonthsBefore = DateTime.Now.AddMonths(-5);
            List<CountingChartDto> countingChartData = new List<CountingChartDto>();
            List<string> monthNames = new List<string>();
            List<EarnCountingChartDto> earnChartData = new List<EarnCountingChartDto>();

            while (lastSixMonthsBefore <= DateTime.Now)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(lastSixMonthsBefore.Month).ToUpper();
                var countingService = serviceList.Where(x => x.HiringDate.Month == lastSixMonthsBefore.Month).Count();
                monthNames.Add(monthName);
                countingChartData.Add(new CountingChartDto(countingService));
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
    }
}
