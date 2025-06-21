using Example_EfCore.Features.Sales.InvoiceOverviews;
using Hydrator.Api;

namespace Example_EfCore.Features.Sales;

public interface IInvoiceOverviewQuery
{
    Task<InvoiceOverviewViewModel> GetInvoiceDetails(Guid invoiceId);

    class Imp : IInvoiceOverviewQuery
    {
        private readonly IInvoiceOverviewsContext _context;
        private readonly IHydratorService _hydrator;

        public Imp(
            IInvoiceOverviewsContext context,
            IHydratorService hydrator)
        {
            _context = context;
            _hydrator = hydrator;
        }
        public async Task<InvoiceOverviewViewModel> GetInvoiceDetails(Guid invoiceId)
        {
            await _context.LoadAsync(invoiceId);
            await _hydrator.HydrateAsync(_context.Result);

            foreach (var detail in _context.Result.InvoiceItemDetails)
            {
                await _hydrator.HydrateAsync(detail);
            }

            return _context.Result;
        }
    }
}