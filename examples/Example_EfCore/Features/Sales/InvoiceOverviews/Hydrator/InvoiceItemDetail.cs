namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public record InvoiceItemDetail : IInvoiceItemDetailQuantityShipped
{
        
    public required Guid LineId { get; init; }

    public required string ProductName { get; init; }

    public required decimal UnitPrice { get; init; }

    public required decimal Amount { get; init; }

    public required int QuantityOrdered { get; init; }

    public required int QuantityShipped { get; set; }

    public int PendingToShip => QuantityOrdered - QuantityShipped;
}