using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.DTOs
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
