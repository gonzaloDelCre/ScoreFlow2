using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTOs
{
    public class UserActionDTO
    {
        public string Action { get; set; }
        public int? UserID { get; set; }
        public UserRequestDTO? User { get; set; }
    }
}
