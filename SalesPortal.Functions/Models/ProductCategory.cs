using System;
using System.Collections.Generic;

namespace SalesPortal.Functions.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? TaxRate { get; set; }

        public static List<ProductCategory> GetProductCategories()
        {
            return new List<ProductCategory> {
                new ProductCategory { Id = 1, Name = "Books", TaxRate = 0},
                new ProductCategory { Id = 2, Name = "Food", TaxRate = 0},
                new ProductCategory { Id = 3, Name = "Medical", TaxRate = 0},
                new ProductCategory { Id = 4, Name = "Other", TaxRate = 0.1},
            };
        }
    }
}
