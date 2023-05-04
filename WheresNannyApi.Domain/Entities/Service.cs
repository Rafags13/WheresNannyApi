using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WheresNannyApi.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public TimeSpan Time;
        public decimal Price{ get; set; }
        public int PersonId { get; set; }
        public Person? PersonService {get; set; }
        public int NannyId { get; set; }
        public Nanny? NannyService { get; set; }
    }
}
