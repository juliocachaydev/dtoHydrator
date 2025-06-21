namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public record PaymentDetail
{
    public required Guid PaymentId { get; init; }

    public required decimal Amount { get; init; }
}