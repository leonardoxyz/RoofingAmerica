using RoofingAmerica.Domain.Core.Data;
using RoofingAmerica.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoofingAmerica.Domain.Services
{
    public class SaleService
    {
        private readonly IRepository<Sale, Guid> _saleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SaleService (IRepository<Sale, Guid> saleRepository, IUnitOfWork unitOfWork)
        {
            _saleRepository = saleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(Guid id)
        {
            return await _saleRepository.GetByIdAsync(id);
        }

        public async Task<Sale> RegisterSaleAsync(Sale sale)
        {
            await _saleRepository.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale> UpdateSaleAsync(Guid id, Sale sale)
        {
            var existingSale = await _saleRepository.GetByIdAsync(id);
            if (existingSale == null) 
            {
                return null;
            }

            existingSale.Cut = sale.Cut;
            existingSale.Quantity = sale.Quantity;
            existingSale.Discount = sale.Discount;
            existingSale.Price = sale.Price;

            await _saleRepository.UpdateAsync(existingSale);
            await _unitOfWork.SaveChangesAsync();
            return existingSale;
        }

        public async Task<bool> DeleteSaleAsync(Guid id)
        {
            await _saleRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
