using MediatR;
using RoofingAmerica.Domain.Models;

namespace RoofingAmerica.Api.Requests
{
    public class UpdateSaleRequest : IRequest<Sale>
    {
        public Guid Id { get; set; }
        public int Cut { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
    }
}
