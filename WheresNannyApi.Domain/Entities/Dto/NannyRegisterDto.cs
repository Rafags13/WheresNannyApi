using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class NannyRegisterDto
    {
        public NannyRegisterDto(float servicePrice, UserRegisterDto userDataToRegister, string base64CriminalRecord, string base64ProofOfAddress) 
        {
            UserDataToRegister = userDataToRegister;
            ServicePrice = servicePrice;
            Base64CriminalRecord = base64CriminalRecord;
            Base64ProofOfAddress = base64ProofOfAddress;
        }
        public NannyRegisterDto() { }
        public float ServicePrice { get; set; }
        public UserRegisterDto UserDataToRegister { get; set;}
        public string Base64CriminalRecord { get; set; } = string.Empty;
        public string Base64ProofOfAddress { get; set; } = string.Empty;
    }
}
