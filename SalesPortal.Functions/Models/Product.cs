using System;
using System.Collections.Generic;

namespace SalesPortal.Functions.Models
{
    public class Product
    {
        private static readonly List<Product> _products = new List<Product> {
                new Product{ Id = 1, Name = "Book", Price = 12.49, IsImported = false, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Books") },
                new Product{ Id = 2, Name = "Music CD", Price = 14.99, IsImported = false, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Other") },
                new Product{ Id = 3, Name = "Chocolate bar", Price = 0.85, IsImported = false, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Food") },
                new Product{ Id = 4, Name = "Imported box of chocolates", Price = 10.00, IsImported = true, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Food") },
                new Product{ Id = 5, Name = "Imported bottle of perfume", Price = 47.50, IsImported = true, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Other") },
                new Product{ Id = 6, Name = "Imported bottle of perfume", Price = 27.99, IsImported = true, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Other") },
                new Product{ Id = 7, Name = "Bottle of perfume", Price = 18.99, IsImported = false, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Other") },
                new Product{ Id = 8, Name = "Package of headache pills", Price = 9.75, IsImported = false, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Medical") },
                new Product{ Id = 9, Name = "Imported box of chocolates", Price = 11.25, IsImported = true, ProductCategory = ProductCategory.GetProductCategories().Find(pc => pc.Name == "Food") },
            };

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsImported { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public static List<Product> GetProducts()
        {
            return _products;
        }

        public static Product GetProductById(int productId)
        {
            return _products.Find(p => p.Id == productId);

        }
    }
}
