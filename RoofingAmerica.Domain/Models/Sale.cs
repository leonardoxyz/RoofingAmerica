using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoofingAmerica.Domain.Models
{
    public class Sale
    {
        public Guid Id { get; set; }
        public int Cut { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
    }
}
