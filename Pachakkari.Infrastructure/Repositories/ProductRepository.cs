using System;
using System.Collections.Generic;
using System.Text;
using Pachakkari.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Pachakkari.Infrastructure.Data;
using Pachakkari.Domain.Entities;

namespace Pachakkari.Infrastructure.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly PachakkariDbContext _context;

        public ProductRepository(PachakkariDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();

        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);

        }

        public async Task<Product>AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;

        }

        public async Task<Product?>UpdateAsync(int id,Product product)
        {
            var existing_product = await _context.Products.FindAsync(id);

            if (existing_product == null)
            {
                return null;

            }
            existing_product.Id = product.Id;
            existing_product.Price = product.Price;
            existing_product.Title = product.Title;
            existing_product.Category = product.Category;
            existing_product.Image = product.Image;
            existing_product.Description = product.Description;
            await _context.SaveChangesAsync();
            return existing_product;
        }

        public async Task<bool>DeleteAsync(int id)
        {
            var product =await  _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;  


        }
       


    }
}
