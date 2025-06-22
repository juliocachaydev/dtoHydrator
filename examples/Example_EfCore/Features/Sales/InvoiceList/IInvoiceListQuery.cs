using Example_EfCore.Infrastructure;
using Hydrator.Api;
using Microsoft.EntityFrameworkCore;

namespace Example_EfCore.Features.Sales.InvoiceList;

public interface IInvoiceListQuery
{
    Task<InvoiceListViewModel> GetInvoiceListAsync();

    class Imp : IInvoiceListQuery
    {
        private readonly AppDbContext _db;
        private readonly IHydratorService _hydratorService;

        public Imp(
            AppDbContext db, 
            IHydratorService hydratorService)
        {
            _db = db;
            _hydratorService = hydratorService;
        }
        public async Task<InvoiceListViewModel> GetInvoiceListAsync()
        {
            var invoices = await _db.Invoices
                .AsNoTracking()
                .ToArrayAsync();

            var result = new InvoiceListViewModel()
            {
                InvoiceLookups = invoices.Select(x =>
                    new InvoiceListViewModel.InvoiceLookup
                    {
                        InvoiceId = x.Id,
                        CustomerId = x.CustomerId,
                        CustomerName = "" // <-- will be hydrated
                    }).ToArray()
            };
            
            // Hydrate the customer names
            await _hydratorService.HydrateAsync(result);

            return result;
        }
    }
}