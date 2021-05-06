using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalesPortal.Functions.Models;


namespace SalesPortal.Functions
{
    public static class ProductsFunction
    {
        [FunctionName("ProductsFuntion")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger products function processed a request.");
            return new OkObjectResult(Product.GetProducts());
        }
    }
}
