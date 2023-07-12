using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Application.Util;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class GenerateTokenUserDto
    {
        public GenerateTokenUserDto(Person personFromToken, DateTime timeToExpire, string deviceId, TypeOfUser type)
        {
            PersonFromToken = personFromToken;
            TimeToExpire = timeToExpire;
            DeviceId = deviceId;
            Type = type;
        }
        public GenerateTokenUserDto() { }
        public Person PersonFromToken { get; set; } = new Person();
        public DateTime TimeToExpire { get; set; } = DateTime.UtcNow;
        public TypeOfUser Type { get; set; }
        public string DeviceId { get; set; } = string.Empty;
    }
}
