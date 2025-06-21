using Hydrator.Api;

namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public interface IInvoiceItemDetailQuantityShipped : IHydratableValue
{
   string ProductName { get; init; }
    int QuantityShipped { get; set; }
    
    public class Handler : IHydratorHandler<IInvoiceItemDetailQuantityShipped>
    {
        private readonly IInvoiceOverviewsContext _context;

        public Handler(IInvoiceOverviewsContext context)
        {
            _context = context;
        }
        
        public Task HydrateAsync(IInvoiceItemDetailQuantityShipped dto)
        {
            var shipments = _context.Shipments
                .Where(x => x.ProductName == dto.ProductName);

            dto.QuantityShipped = shipments.Sum(x => x.Quantity);
            
            return Task.CompletedTask;
        }
    }
}
