using Example_EfCore.Domain;
using Example_EfCore.Features.Sales.InvoiceOverviews;
using Moq;

namespace Example_EfCore.tests.Features.Sales.Hydrator;

public class IInvoicePaymentsTests
{
    private readonly Mock<IInvoiceOverviewsContext> _contextMock;

    public IInvoicePaymentsTests()
    {
        _contextMock = new();
    }
    
    private IInvoicePayments.Handler CreateSut()
    {
        return new IInvoicePayments.Handler(_contextMock.Object);
    }

    [Fact]
    public async Task MapsPaymentsForInvoice()
    {
        // ***** ARRANGE *****
        
        var sut = CreateSut();

        var dto = new Dto()
        {
            InvoiceId = Guid.NewGuid(),
            Payments = []
        };

        var payment1 = new Payment(Guid.NewGuid(), dto.InvoiceId, 100);
        var payment2 = new Payment(Guid.NewGuid(), dto.InvoiceId, 200);
        
        _contextMock.Setup(x=>
            x.Payments).Returns([payment1, payment2]);
        
        // ***** ACT *****

        await sut.HydrateAsync(dto);

        // ***** ASSERT *****
        
        Assert.Equal(2, dto.Payments.Length);
        
        Assert.Contains(dto.Payments, x=>
            x.PaymentId == payment1.Id && x.Amount == payment1.Amount);
        
        Assert.Contains(dto.Payments, x=>
            x.PaymentId == payment2.Id && x.Amount == payment2.Amount);
    }

    

    class Dto : IInvoicePayments
    {
        public Guid InvoiceId { get; set; }
        public PaymentDetail[] Payments { get; set; }
    }
}