using Example_EfCore.Features.Sales.InvoiceList.Hydrator;

namespace Example_EfCore.Features.Sales.InvoiceList;

public record InvoiceListViewModel : ICustomerNames
{
    public IEnumerable<InvoiceLookup> InvoiceLookups { get; init; } = [];
    public record InvoiceLookup
    {
        public required Guid InvoiceId { get; init; }

        public required Guid CustomerId { get; init; }

        public string CustomerName { get; set; } = ""; // <-- hydrated value
    }
}