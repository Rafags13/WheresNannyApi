using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class NannyContractDto
    {
        public NannyContractDto () { }
        public int NannyId { get; set; }
        public string ImageProfileBase64Uri { get; set; }
        public float RankAverageStars { get; set; }
        public int RankCommentCount { get; set; }
        public float ServicePrice { get; set; }
        public NannyPersonContractDto Person { get; set; }
        public NannyAddressPersonContractDto Address { get; set; }
    }

    public class NannyPersonContractDto
    {
        public string Cellphone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class NannyAddressPersonContractDto
    {
        public string Cep { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;

        public string DistanceBetweenThePeople { get; set; } = string.Empty;
    }
}
