namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public interface IInvoicePayments
{
    Guid InvoiceId { get;}
    PaymentDetail[] Payments { get; set; }
}


public record PaymentDetail
{
    public required Guid PaymentId { get; init; }

    public required decimal Amount { get; init; }
}