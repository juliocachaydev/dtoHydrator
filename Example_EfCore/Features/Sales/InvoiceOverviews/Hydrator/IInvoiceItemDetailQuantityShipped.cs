namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public interface IInvoiceItemDetailQuantityShipped
{
    Guid InvoiceId { get;}
    Guid LineId { get; }
    int QuantityShipped { get; init; }
}