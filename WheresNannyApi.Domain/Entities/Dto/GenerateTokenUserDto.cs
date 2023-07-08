using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class GenerateTokenUserDto
    {
        public GenerateTokenUserDto(Person personFromToken, DateTime timeToExpire, string deviceId)
        {
            PersonFromToken = personFromToken;
            TimeToExpire = timeToExpire;
            DeviceId = deviceId;
        }
        public GenerateTokenUserDto() { }
        public Person PersonFromToken { get; set; } = new Person();
        public DateTime TimeToExpire { get; set; } = DateTime.UtcNow;
        public string DeviceId { get; set; } = string.Empty;
    }
}
