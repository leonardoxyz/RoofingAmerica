using System;

namespace RoofingAmerica.Domain.Models
{
    public class Sale
    {
        public Guid Id { get; set; }
        public int Cut { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        private double discount;
        private double price;

        public double Discount
        {
            get { return discount; }
            set { discount = Math.Round(value, 2); }
        }

        public double Price
        {
            get { return price; }
            set { price = Math.Round(value, 2); }
        }
        public string FormattedDiscount => Discount.ToString("C2");
        public string FormattedPrice => Price.ToString("C2");
    }
}
