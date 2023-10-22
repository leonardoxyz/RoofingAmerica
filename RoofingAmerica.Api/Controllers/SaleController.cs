using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoofingAmerica.Api.Requests;
using RoofingAmerica.Domain.Models;
using RoofingAmerica.Domain.Services;

namespace RoofingAmerica.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/Sale")]
    public class SaleController : Controller
    {
        private readonly SaleService _saleService;
        private readonly IMediator _mediator;

        public SaleController(SaleService saleService, IMediator mediator)
        {
            _saleService = saleService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSale(Sale sale)
        {
            var registeredSale = await _saleService.RegisterSaleAsync(sale);
            return CreatedAtAction(nameof(RegisterSale), new { id = registeredSale.Id }, registeredSale);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSale(Guid id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(sale);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Sale>> UpdateSale(Guid id, [FromBody] UpdateSaleRequest request)
        {
            request.Id = id;

            var updatedSale = await _mediator.Send(request);
            if (updatedSale == null)
            {
                return NotFound();
            }

            return Ok(updatedSale);
        }

        [HttpDelete("{id}")] // Corrigi o caminho do parâmetro "id"
        public async Task<IActionResult> DeleteSale(Guid id)
        {
            var existingSale = await _saleService.DeleteSaleAsync(id);
            if (!existingSale)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
