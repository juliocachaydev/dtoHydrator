using Example_EfCore.Features.Sales.InvoiceOverviews;

namespace Example_EfCore.Features.Sales;

public interface IInvoiceOverviewQuery
{
    Task<InvoiceItemDetail[]> GetInvoiceDetails();

    class Imp : IInvoiceOverviewQuery
    {
        public Task<InvoiceItemDetail[]> GetInvoiceDetails()
        {
            throw new NotImplementedException();
        }
    }
}