namespace Example_EfCore.Domain;

public class Invoice
{
    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public IEnumerable<InvoiceLine> Lines { get; private set; } = new List<InvoiceLine>();

    // Ef Core
    private Invoice()
    {
        
    }

    public Invoice(Guid id, Guid customerId)
    {
        Id = id;
        CustomerId = customerId;
    }
    
    public void AddLine(Guid lineId, string productName, int quantity, decimal unitPrice)
    {

        var line = new InvoiceLine(lineId, productName, quantity, unitPrice);

        ((List<InvoiceLine>)Lines).Add(line);
    }

    public class InvoiceLine
    {
        public Guid Id { get; private set; }

        public string ProductName { get; private set; }
        
        public int Quantity { get; private set; }
        
        public decimal UnitPrice { get; private set; }
        
        public decimal Amount => Quantity * UnitPrice;

        // ef core
        private InvoiceLine()
        {
            
        }

        public InvoiceLine(Guid id, string productName, int quantity, decimal unitPrice)
        {
            Id = id;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}