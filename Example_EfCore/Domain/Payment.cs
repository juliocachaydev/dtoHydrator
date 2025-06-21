namespace Example_EfCore.Domain;

public class Payment
{
    public Guid Id { get; private set; }

    public Guid InvoiceId { get; private set; }
    
    public decimal Amount { get; private set; }

    public Payment(Guid id, Guid invoiceId, decimal amount)
    {
        Id = id;
        InvoiceId = invoiceId;
        Amount = amount;
    }
}