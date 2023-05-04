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
            string imageUri,
            int userId
            ) 
        {
            Fullname = fullname;
            Email = email;
            Cellphone = cellphone;
            BirthdayDate = birthdayDate;
            Cpf = cpf;
            ImageUri = imageUri;
            UserId = userId;
        }

        public Person() { }
        public int Id { get; set; }
        public string Fullname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Cellphone { get; set; } = String.Empty;
        public DateTime BirthdayDate { get; set; }
        public string Cpf { get; set; } = String.Empty;
        public string ImageUri { get; set; } = String.Empty;
        public int UserId { get; set; }
        public User? User { get; set; }
        public Address? Address { get; set; }
        public Nanny? Nanny { get; set; }
        public ICollection<Service>? ServicesPerson { get; set; }
    }
}
