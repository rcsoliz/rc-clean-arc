﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; }
    }
}
