using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class NannyRegisterDocumentDto
    {
        public NannyRegisterDocumentDto(int personId, string base64CriminalRecord, string base64ProofOfAddress) 
        {
            PersonId = personId;
            Base64CriminalRecord = base64CriminalRecord;
            Base64ProofOfAddress = base64ProofOfAddress;
        }
        public NannyRegisterDocumentDto() { }
        public int PersonId { get; set; }
        public string Base64CriminalRecord { get; set; }
        public string Base64ProofOfAddress { get; set; }
    }
}
