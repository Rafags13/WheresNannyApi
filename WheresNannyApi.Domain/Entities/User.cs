using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class User
    {
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public DateTime? CreatedIn { get; set; }
        public Person? Person { get; set; }  
    }
}
