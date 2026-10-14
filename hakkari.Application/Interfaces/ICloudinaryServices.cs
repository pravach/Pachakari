using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Pachakkari.Application.Interfaces
{
    public interface ICloudinaryServices
    {
        Task<string> UploadImageAsync(IFormFile image);
    }
}
