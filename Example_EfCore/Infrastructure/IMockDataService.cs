using Example_EfCore.Domain;

namespace Example_EfCore.Infrastructure;

public interface IMockDataService
{
    Task CreateMockDataAsync();

    class Imp : IMockDataService
    {
        private readonly AppDbContext _db;

        public Imp(AppDbContext db)
        {
            _db = db;
        }
        
        public async Task CreateMockDataAsync()
        {
            var invoice = new Invoice(Guid.NewGuid());
            
            invoice.AddLine(Guid.NewGuid(), "Bolt #4 x box of 100", 100, 9.99m);
            invoice.AddLine(Guid.NewGuid(), "Nut #4 x box of 100", 50, 4.99m);

            var payment1 = new Payment(Guid.NewGuid(), invoice.Id, 999.99m);
            var payment2 = new Payment(Guid.NewGuid(), invoice.Id, 328.99m);
            
            var shipment1 = new Shipment(
                Guid.NewGuid(), invoice.Id,
                "Bolt #4 x box of 100", 50);
            
            var shipment2 = new Shipment(
                Guid.NewGuid(), invoice.Id,
                "Nut #4 x box of 100", 50);
            
            _db.Invoices.Add(invoice);
            _db.Payments.Add(payment1);
            _db.Payments.Add(payment2);
            _db.Shipments.Add(shipment1);
            _db.Shipments.Add(shipment2);

            await _db.SaveChangesAsync();
        }
    }
}