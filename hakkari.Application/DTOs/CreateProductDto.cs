using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.DTOs
{
    public class CreateProductDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}
