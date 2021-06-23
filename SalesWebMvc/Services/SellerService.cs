using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller objSeller)
        {
            _context.Add(objSeller); // Add() é somente feito em memória
            await _context.SaveChangesAsync(); // o que realmente vai acessar o BD
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            // eager loading
            return await _context.Seller.Include(obj => obj.Department)
                                        .FirstOrDefaultAsync(obj => obj.Id == id); 
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAnySeller = await _context.Seller.AnyAsync(x => x.Id == obj.Id);

            if (!hasAnySeller)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                // Interceptando uma exceção do nível de acesso a dados, mas relançando-a a nível de serviço
                // segregando as camadas. A camada de serviço não vai propagar uma exceção da camada de acesso a dados
                throw new DbConcurrencyException(e.Message);
            }
            
        }
    }
}
