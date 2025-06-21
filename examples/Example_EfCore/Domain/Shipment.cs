namespace Example_EfCore.Domain;

public class Shipment
{
    public Guid Id { get; private set; }

    public Guid InvoiceId { get; private set; }

    public string ProductName { get; private set; }

    public int Quantity { get; private set; }

    public Shipment(Guid id, Guid invoiceId, string productName, int quantity)
    {
        Id = id;
        InvoiceId = invoiceId;
        ProductName = productName;
        Quantity = quantity;
    }
}