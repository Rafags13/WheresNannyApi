using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class NannyCardDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public float StarsCounting { get; set; }
        public int RankCommentCount { get; set; }
        public string ImageUri { get; set; } = string.Empty;
    }
}
