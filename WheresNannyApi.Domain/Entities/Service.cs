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
        public Service(TimeSpan serviceFinishHour, DateTime hiringDate, decimal price, int personId, int nannyId)
        {
            ServiceFinishHour = serviceFinishHour;
            HiringDate = hiringDate;
            Price = price;
            PersonId = personId;
            NannyId = nannyId;
        }
        public int Id { get; set; }
        public TimeSpan ServiceFinishHour { get; set; }
        public DateTime HiringDate { get; set; }
        public decimal Price{ get; set; }
        public int PersonId { get; set; }
        public bool ServiceAccepted { get; set; }
        public Person PersonService { get; set; } = new Person();
        public int NannyId { get; set; }
        public Nanny NannyService { get; set; } = new Nanny();
    }
}
