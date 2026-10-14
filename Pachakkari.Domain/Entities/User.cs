using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Type { get; set; }
        public bool IsBlocked { get; set; } = false;
    }
}
