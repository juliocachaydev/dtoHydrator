namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public interface IInvoiceItemDetailsRootData
{
    public Guid InvoiceId { get;}
    InvoiceItemDetail[] InvoiceItemDetails { get; set; }
}

public record InvoiceItemDetail : IInvoiceItemDetailQuantityShipped
{
    public required Guid InvoiceId { get; init; }
        
    public required Guid LineId { get; init; }

    public required string ProductName { get; init; }

    public required decimal UnitPrice { get; init; }

    public required decimal Amount { get; init; }

    public required int QuantityOrdered { get; init; }

    public required int QuantityShipped { get; init; }

    public int PendingToShip => QuantityOrdered - QuantityShipped;
}