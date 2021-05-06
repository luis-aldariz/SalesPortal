using System;
namespace SalesPortal.Functions.Models.Response
{
    public class SalesTaxesResponse
    {
        public Receipt Receipt { get; }

        public SalesTaxesResponse()
        {
            Receipt = new Receipt();
        }
    }
}
