﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMovil.Models
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
