using Pachakkari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
