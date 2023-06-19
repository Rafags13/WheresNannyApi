using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities.Dto
{
    public class UpdatePasswordDto
    {
        public UpdatePasswordDto(int userId, string oldPassword, string newPassword) 
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public UpdatePasswordDto() { }
        public int UserId { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
