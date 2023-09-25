using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class Address
    {
        public Address(
            string cep,
            string houseNumber,
            string complement,
            float latitude = 0.0f,
            float longitude = 0.0f
            )
        {
            Cep = cep;
            HouseNumber = houseNumber;
            Complement = complement;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Address() { }
        public int Id { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string Complement { get; set; } = string.Empty;
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public ICollection<Person>? PersonsWhoHasThisAddress { get; set; } = new List<Person>();
    }
}
