using Hydrator.Api;

namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public interface IInvoiceItemDetailsRootData : IHydratableValue
{
    public Guid InvoiceId { get;}
    InvoiceItemDetail[] InvoiceItemDetails { get; set; }

    class Handler : IHydratorHandler<IInvoiceItemDetailsRootData>
    {
        private readonly IInvoiceOverviewsContext _context;

        public Handler(IInvoiceOverviewsContext context)
        {
            _context = context;
        }
        
        public Task HydrateAsync(IInvoiceItemDetailsRootData dto)
        {
            var details = _context.Invoice.Lines.Select(l =>
                new InvoiceItemDetail
                {
                    LineId = l.Id,
                    ProductName = l.ProductName,
                    UnitPrice = l.UnitPrice,
                    Amount = l.Amount,
                    QuantityOrdered = l.Quantity,
                    QuantityShipped = 0 // <-- this one is set by another hydrator
                }).ToArray();
            
            dto.InvoiceItemDetails = details;
            
            return Task.CompletedTask;
        }
    }
}