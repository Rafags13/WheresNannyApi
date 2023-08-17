using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class PointsInMapDto
    {
        public PointsInMapDto(
            CoordinateDto originCoordinate,
            CoordinateDto finalCoordinate)
        {
            OriginCoordinate = originCoordinate;
            FinalCoordinate = finalCoordinate;
        }
        public PointsInMapDto() { }

        public CoordinateDto OriginCoordinate { get; set; } = new CoordinateDto();
        public CoordinateDto FinalCoordinate { get; set; } = new CoordinateDto();
    }
}
