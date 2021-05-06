using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesPortal.Functions.Models
{
    public class Receipt
    {
        public List<TotalItem> TotalItems { get; set; }
        public double SalesTaxes { get { return CalculateSalesTaxes(); } }
        public double Total { get { return CalculateTotal(); } }

        public Receipt()
        {
            TotalItems = new List<TotalItem>();
        }

        private double CalculateSalesTaxes()
        {
            return TotalItems.Sum(id => id.SaleTax);
        }

        private double CalculateTotal()
        {
            return TotalItems.Sum(id => id.Total);
        }
    }
}
