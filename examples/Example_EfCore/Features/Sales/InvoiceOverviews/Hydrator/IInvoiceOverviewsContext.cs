using Example_EfCore.Domain;
using Example_EfCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Example_EfCore.Features.Sales.InvoiceOverviews;

/// <summary>
/// A scoped service that allows me to put all the data I need to hydrate the view-model and an empty view-model that
/// will be hydrated later.
/// </summary>
public interface IInvoiceOverviewsContext
{
    public Invoice Invoice { get; }
    
    public IEnumerable<Payment> Payments { get; }
    
    public IEnumerable<Shipment> Shipments { get; }
    
    public InvoiceOverviewViewModel Result { get; }

    Task LoadAsync(Guid invoiceId);

    class Imp : IInvoiceOverviewsContext
    {
        private readonly AppDbContext _db;

        public Imp(AppDbContext db)
        {
            _db = db;
        }
        public Invoice Invoice { get; private set; }
        public IEnumerable<Payment> Payments { get; private set; } = [];
        public IEnumerable<Shipment> Shipments { get; private set; } = [];
        public InvoiceOverviewViewModel Result { get; private set; }

        public async Task LoadAsync(Guid invoiceId)
        {
            Invoice = await _db.Invoices.AsNoTracking()
                .Include(e => e.Lines)
                .FirstAsync(x=> x.Id == invoiceId);

            Payments = await _db.Payments.AsNoTracking()
                .Where(x=> x.InvoiceId == invoiceId)
                .ToArrayAsync();

            Shipments = await _db.Shipments.AsNoTracking()
                .Where(x=> x.InvoiceId == invoiceId)
                .ToArrayAsync();

            Result = new()
            {
                InvoiceId = invoiceId,
                InvoiceItemDetails = [],
                Payments = []
            };
        }
    }
}