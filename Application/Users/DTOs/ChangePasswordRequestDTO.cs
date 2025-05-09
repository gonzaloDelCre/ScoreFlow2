using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTOs
{
    public class ChangePasswordRequestDTO
    {
        public string CurrentPasswordHash { get; set; } = null!;
        public string NewPasswordHash { get; set; } = null!;
    }
}
