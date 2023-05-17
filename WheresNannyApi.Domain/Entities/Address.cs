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
            string complement
            )
        {
            Cep = cep;
            HouseNumber = houseNumber;
            Complement = complement;
        }
        public int Id { get; set; }
        public string Cep { get; set; }
        public string HouseNumber { get; set; }
        public string Complement { get; set; }
        public ICollection<Person>? PersonsWhoHasThisAddress { get; set; }
    }
}
