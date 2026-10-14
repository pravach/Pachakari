using System;
using System.Collections.Generic;
using System.Text;
using Pachakkari.Domain.Entities;

namespace Pachakkari.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(int id, Product product);
        Task<bool> DeleteAsync(int id);

    }
}
