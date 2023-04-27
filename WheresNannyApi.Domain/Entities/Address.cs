using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Cep { get; set; }
        public string HouseNumber { get; set; }
        public string Complement { get; set; }
        public int personId { get; set; }
        public Person Person { get; set; }
    }
}
