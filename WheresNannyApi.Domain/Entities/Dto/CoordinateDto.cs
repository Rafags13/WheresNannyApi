using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class CoordinateDto
    {
        public CoordinateDto() { }

        public CoordinateDto(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
