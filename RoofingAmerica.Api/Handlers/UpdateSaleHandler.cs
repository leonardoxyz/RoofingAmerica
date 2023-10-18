using MediatR;
using RoofingAmerica.Api.Requests;
using RoofingAmerica.Domain.Core.Data;
using RoofingAmerica.Domain.Models;

namespace RoofingAmerica.Api.Handlers
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleRequest, Sale>
    {
        private readonly IRepository<Sale, Guid> _saleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSaleHandler(IRepository<Sale, Guid> saleRepository, IUnitOfWork unitOfWork) 
        {
            _saleRepository = saleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Sale> Handle(UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            var existingSale = await _saleRepository.GetByIdAsync(request.Id);
            if (existingSale == null)
            {
                return null;
            }

            existingSale.Cut = request.Cut;
            existingSale.Quantity = request.Quantity;
            existingSale.Discount = request.Discount;
            existingSale.Price = request.Price;

            await _saleRepository.UpdateAsync(existingSale);
            await _unitOfWork.SaveChangesAsync();
            return existingSale;
        }
    }
}
