using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SalesPortal.Functions.Models;
using SalesPortal.Functions.Extensions;
using SalesPortal.Functions.Models.Request;
using SalesPortal.Functions.Models.Response;
namespace SalesPortal.Functions
{
    public static class SalesTaxesFunction
    {
        const double _importedTaxRate = 0.05;

        [FunctionName("SalesTaxesFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger sale taxes function processed a request.");

            var salesTaxesResponse = new SalesTaxesResponse();

            try
            {
                var salesTaxesBody = await new StreamReader(req.Body).ReadToEndAsync();
                var salesTaxesRequest = JsonConvert.DeserializeObject<SalesTaxesRequest>(salesTaxesBody);
                if (salesTaxesRequest.Items == null || salesTaxesRequest.Items.Count == 0)
                    return new BadRequestObjectResult("Please provide a valid request");

                salesTaxesRequest.Items.GroupBy(item => item.ProductId).ToList().ForEach(item => {
                    var product = Product.GetProductById(item.First().ProductId);
                    var itemSaleTax = CalculateItemSaleTax(product);
                    var quantity = item.Count();

                    salesTaxesResponse.Receipt.TotalItems.Add(new TotalItem
                    {
                        Quantity = quantity,
                        ProductName = product.Name,
                        SaleTax = itemSaleTax * quantity,
                        UnitPrice = Math.Round(product.Price + itemSaleTax, 2)
                    });
                });

                return new OkObjectResult(salesTaxesResponse);
            }
            catch (Exception ex)
            {
                log.LogInformation($"Exception : { ex.Message }");
                return new BadRequestObjectResult("An exception occurred trying to get the sales taxes");
            }
        }

        private static double CalculateItemSaleTax(Product product)
        {
            var basicTax = (product.ProductCategory.TaxRate.Value * product.Price).RoundNearestFiveCents();
            var totalTax = product.IsImported
                           ? basicTax + (product.Price * _importedTaxRate).RoundNearestFiveCents()
                           : basicTax;

            return Math.Round(totalTax, 2);
        }
    }
}
