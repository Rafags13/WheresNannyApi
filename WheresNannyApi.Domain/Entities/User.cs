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

        public User() { }
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public DateTime? CreatedIn { get; set; }
        public Person Person { get; set; } = new Person();
    }
}
