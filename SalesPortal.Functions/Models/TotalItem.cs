using System;
namespace SalesPortal.Functions.Models
{
    public class TotalItem
    {
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double SaleTax { get; set; }
        public double Total { get { return CalculateTotal(); } }

        private double CalculateTotal()
        {
            return Math.Round(UnitPrice * Quantity, 2);
        }
    }
}
