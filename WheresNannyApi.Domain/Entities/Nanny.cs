using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class Nanny : Person
    {
        public Nanny(float servicePrice)
        {
            ServicePrice = servicePrice;
        }
        public Nanny() { }
        public float ServicePrice { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public ICollection<Service>? ServicesNanny { get; set; }
    }
}
