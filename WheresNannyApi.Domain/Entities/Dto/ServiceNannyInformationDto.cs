using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class ServiceNannyInformationDto
    {
        public ServiceNannyInformationDto(
            string parentName,
            string parentPictureBase64,
            string parentCep,
            TimeSpan serviceFinishHour,
            CoordinateDto originCoordinates,
            CoordinateDto destinationCoordinates,
            decimal servicePrice) 
        {
            ParentName = parentName;
            ParentPictureBase64 = parentPictureBase64;
            ParentCep = parentCep;
            OriginCoordinates = originCoordinates;
            DestinationCoordinates = destinationCoordinates;
            ServiceFinishHour = serviceFinishHour;
            ServicePrice = servicePrice;
        }
        public ServiceNannyInformationDto() { }
        
        public string ParentName { get; set; } = string.Empty;
        public string ParentCellphone { get; set; } = string.Empty;
        public string ParentEmail { get; set; } = string.Empty;
        public DateTime ParentBirthdayDate { get; set; }
        public string ParentPictureBase64 { get; set; } = string.Empty;
        public string ParentCep { get; set; } = string.Empty;
        public CoordinateDto OriginCoordinates { get; set; } = new CoordinateDto();
        public CoordinateDto DestinationCoordinates { get; set; } = new CoordinateDto();
        public TimeSpan ServiceFinishHour { get; set; }
        public decimal ServicePrice { get; set; }
        public double Distance { get; set; }

    }
}
