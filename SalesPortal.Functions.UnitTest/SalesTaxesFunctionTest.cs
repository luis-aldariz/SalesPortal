using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using System.IO;
using System.Text;
using SalesPortal.Functions.Models.Response;


namespace SalesPortal.Functions.UnitTest
{
    public class SalesTaxesFunctionTest
    {
        [Fact]
        public void valid_request_should_be__success()
        {
            var items = "{ 'Items':[{ 'ProductId':1, 'Quantity': 1}]}";
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = new MemoryStream(Encoding.UTF8.GetBytes(items))
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = SalesTaxesFunction.Run(request, logger);
            response.Wait();

            // Check that the response is an "OK" response
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents
            var result = (OkObjectResult)response.Result;
            var receipt = ((SalesTaxesResponse)result.Value).Receipt;

            Assert.Single(receipt.TotalItems);
        }

        [Fact]
        public void case_input_one_shouldbe__success()
        {
            var items = "{ 'Items':[{ 'ProductId':1, 'Quantity': 1}," +
                                   "{ 'ProductId':1, 'Quantity': 1}," +
                                   "{ 'ProductId':2, 'Quantity': 1}," +
                                   "{ 'ProductId':3, 'Quantity': 1}" +
                        "]}";
            var request = new DefaultHttpRequest(new DefaultHttpContext()) { Body = new MemoryStream(Encoding.UTF8.GetBytes(items)) };
            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = SalesTaxesFunction.Run(request, logger);
            response.Wait();

            // Check that the response is an "OK" response
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents            
            var result = (OkObjectResult)response.Result;
            var receipt = ((SalesTaxesResponse)result.Value).Receipt;
            var productSample = receipt.TotalItems.Find(ti => ti.ProductName == "Book");

            Assert.Equal(3, receipt.TotalItems.Count);
            Assert.Equal(1.50, receipt.SalesTaxes);
            Assert.Equal(42.32, receipt.Total);

            Assert.NotNull(productSample);
            Assert.Equal(12.49, productSample.UnitPrice);
            Assert.Equal(2, productSample.Quantity);
            Assert.Equal(0, productSample.SaleTax);
            Assert.Equal(24.98, productSample.Total);
        }

        [Fact]
        public void case_input_two_shouldbe__success()
        {
            var items = "{ 'Items':[{ 'ProductId':4, 'Quantity': 1}," +
                                   "{ 'ProductId':5, 'Quantity': 1}" +
                        "]}";
            var request = new DefaultHttpRequest(new DefaultHttpContext()) { Body = new MemoryStream(Encoding.UTF8.GetBytes(items)) };
            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = SalesTaxesFunction.Run(request, logger);
            response.Wait();

            // Check that the response is an "OK" response
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents            
            var result = (OkObjectResult)response.Result;
            var receipt = ((SalesTaxesResponse)result.Value).Receipt;
            var productSample = receipt.TotalItems.Find(ti => ti.ProductName == "Imported box of chocolates");

            Assert.Equal(2, receipt.TotalItems.Count);
            Assert.Equal(7.65, receipt.SalesTaxes);
            Assert.Equal(65.15, receipt.Total);

            Assert.NotNull(productSample);
            Assert.Equal(10.50, productSample.UnitPrice);
            Assert.Equal(1, productSample.Quantity);
            Assert.Equal(0.5, productSample.SaleTax);
            Assert.Equal(10.50, productSample.Total);
        }

        [Fact]
        public void case_input_three_shouldbe__success()
        {
            var items = "{ 'Items':[{ 'ProductId':6, 'Quantity': 1}," +
                                   "{ 'ProductId':7, 'Quantity': 1}," +
                                   "{ 'ProductId':8, 'Quantity': 1}," +
                                   "{ 'ProductId':9, 'Quantity': 1}," +
                                   "{ 'ProductId':9, 'Quantity': 1}" +
                        "]}";
            var request = new DefaultHttpRequest(new DefaultHttpContext()) { Body = new MemoryStream(Encoding.UTF8.GetBytes(items)) };
            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = SalesTaxesFunction.Run(request, logger);
            response.Wait();

            // Check that the response is an "OK" response
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents            
            var result = (OkObjectResult)response.Result;
            var receipt = ((SalesTaxesResponse)result.Value).Receipt;
            var productSample = receipt.TotalItems.Find(ti => ti.ProductName == "Package of headache pills");

            Assert.Equal(4, receipt.TotalItems.Count);
            Assert.Equal(7.30, receipt.SalesTaxes);
            Assert.Equal(86.53, receipt.Total);

            Assert.NotNull(productSample);
            Assert.Equal(9.75, productSample.UnitPrice);
            Assert.Equal(1, productSample.Quantity);
            Assert.Equal(0, productSample.SaleTax);
            Assert.Equal(9.75, productSample.Total);
        }

        [Fact]
        public void bad_request_should_be__failure()
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = SalesTaxesFunction.Run(request, logger);
            response.Wait();

            // Check that the response is an "Bad" response
            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents
            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal("An exception occurred trying to get the sales taxes", result.Value);
        }

        [Fact]
        public void empty_items_request_should_be__failure()
        {
            var items = "{ 'Items':[]}";
            var request = new DefaultHttpRequest(new DefaultHttpContext()) { Body = new MemoryStream(Encoding.UTF8.GetBytes(items)) };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = SalesTaxesFunction.Run(request, logger);
            response.Wait();

            // Check that the response is an "Bad" response
            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents
            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal("Please provide a valid request", result.Value);
        }
    }
}
