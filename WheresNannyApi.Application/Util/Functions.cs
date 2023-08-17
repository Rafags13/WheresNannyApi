using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Util
{
    public class Functions
    {
        public static string Sha1Encrypt(string input)
        {
            var sha1 = SHA1.Create();
            var inputEncrypted = Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input)));
            return inputEncrypted;
        }


        private static double CalculateDistaceBetweenTwoPoints(PointsInMapDto points)
        {
            GeoCoordinate currentPersonCoordinate = new GeoCoordinate(points.OriginCoordinate.Latitude, points.OriginCoordinate.Longitude);
            GeoCoordinate nannyPersonCoordinate = new GeoCoordinate(points.FinalCoordinate.Latitude, points.FinalCoordinate.Longitude);

            return currentPersonCoordinate.GetDistanceTo(nannyPersonCoordinate);
        }

        public static double GetDistanceBetweenTwoPoints(Address originAddress, Address finalAddress)
        {
            var firstCoordinate = new CoordinateDto
            {
                Latitude = originAddress.Latitude ?? 0.0f,
                Longitude = originAddress.Longitude ?? 0.0f,
            };

            var secondCoordinate = new CoordinateDto
            {
                Latitude = finalAddress.Latitude ?? 0.0f,
                Longitude = finalAddress.Longitude ?? 0.0f,
            };

            var points = new PointsInMapDto { OriginCoordinate = firstCoordinate, FinalCoordinate = secondCoordinate };

            var distance = CalculateDistaceBetweenTwoPoints(points);

            return distance;
        }
    }
}
