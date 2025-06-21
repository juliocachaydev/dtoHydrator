namespace Example_EfCore.Features.Sales.InvoiceOverviews;

public record InvoiceOverviewViewModel : IInvoiceItemDetailsRootData, IInvoicePayments
{
    public required Guid InvoiceId { get; init; }

    public decimal TotalAmount => InvoiceItemDetails.Sum(x => x.Amount);

    public decimal TotalPaid => Payments.Sum(p => p.Amount);

    public decimal Balance => TotalAmount - TotalPaid;

    // Set by the hydrator for InvoiceItemDetailsRootData
    public InvoiceItemDetail[] InvoiceItemDetails { get; set; } = [];

    // Set by the hydrator for PaymentDetails
    public PaymentDetail[] Payments { get; set; } = [];
    
}