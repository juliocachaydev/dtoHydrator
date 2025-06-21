using Hydrator.Api;

namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public interface IInvoicePayments : IHydratableValue
{
    Guid InvoiceId { get;}
    PaymentDetail[] Payments { get; set; }

    class Handler : IHydratorHandler<IInvoicePayments>
    {
        private readonly IInvoiceOverviewsContext _context;

        public Handler(IInvoiceOverviewsContext context)
        {
            _context = context;
        }
        
        public Task HydrateAsync(IInvoicePayments dto)
        {
            var payments = _context.Payments
                .Select(p => new PaymentDetail
                {
                    PaymentId = p.Id,
                    Amount = p.Amount
                }).ToArray();
            
            dto.Payments = payments;
            
            return Task.CompletedTask;
        }
    }
}