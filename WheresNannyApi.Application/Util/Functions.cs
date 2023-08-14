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

        public static double DistanceBetweenTwoPoints(CoordinateDto firstCoordinate, CoordinateDto secondCoordinate)
        {
            GeoCoordinate currentPersonCoordinate = new GeoCoordinate(firstCoordinate.Latitude, firstCoordinate.Longitude);
            GeoCoordinate nannyPersonCoordinate = new GeoCoordinate(secondCoordinate.Latitude, secondCoordinate.Longitude);

            return currentPersonCoordinate.GetDistanceTo(nannyPersonCoordinate);
        }
    }
}
