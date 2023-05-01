using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class Person
    {
        public Person(
            string fullname,
            string email,
            string cellphone, 
            DateTime birthdayDate, 
            string cpf,
            bool isNanny,
            int userId
            ) 
        {
            Fullname = fullname;
            Email = email;
            Cellphone = cellphone;
            BirthdayDate = birthdayDate;
            Cpf = cpf;
            IsNanny = isNanny;
            UserId = userId;
        }
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string Cpf { get; set; }
        public bool IsNanny { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public Address? Address { get; set; }
        public ICollection<Service>? ServicesPerson { get; set; }
        public ICollection<Service>? ServiceNanny { get; set; }
    }
}
