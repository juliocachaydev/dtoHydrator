using Example_EfCore.Infrastructure;
using Hydrator.Api;
using Microsoft.EntityFrameworkCore;

namespace Example_EfCore.Features.Sales.InvoiceList.Hydrator;

public interface ICustomerNames : IHydratableValue
{
    public IEnumerable<InvoiceListViewModel.InvoiceLookup> InvoiceLookups { get; }

    class Handler : IHydratorHandler<ICustomerNames>
    {
        private readonly AppDbContext _db;

        public Handler(AppDbContext db)
        {
            _db = db;
        }
        public async Task HydrateAsync(ICustomerNames dto)
        {
            var customerIds = dto.InvoiceLookups.Select(x => x.CustomerId)
                .ToArray();
            
            var customerNames = await _db.Customers
                .AsNoTracking()
                .Where(x => customerIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x.Name);

            foreach (var invoiceLookup in dto.InvoiceLookups)
            {
                invoiceLookup.CustomerName = customerNames[invoiceLookup.CustomerId];
            }
        }
    }
}