using System.Reflection;
using Example_EfCore.Domain;
using Example_EfCore.Features.Sales.InvoiceOverviews;
using Moq;

namespace Example_EfCore.tests.Features.Sales.Hydrator;

public class IInvoiceItemDetailsRootDataTests
{
    private readonly Mock<IInvoiceOverviewsContext> _contextMock;

    public IInvoiceItemDetailsRootDataTests()
    {
        _contextMock = new Mock<IInvoiceOverviewsContext>();
    }
    
    private IInvoiceItemDetailsRootData.Handler CreateSut()
    {
        return new (_contextMock.Object);
    }

    [Fact]
    public async Task SetsOneDetailForEachInvoiceLine()
    {
        // ***** ARRANGE *****

        var invoiceId = Guid.NewGuid();

        var dto = new Dto
        {
            InvoiceId = invoiceId,
            InvoiceItemDetails = []
        };

        var sut = CreateSut();

        var invoice = new Invoice(invoiceId);

        var line1Id = Guid.NewGuid();
        var line2Id = Guid.NewGuid();
        
        invoice.AddLine(line1Id, "Bolts", 100, 10);
        invoice.AddLine(line2Id, "Nuts", 100, 10);

        var line1 = invoice.Lines.First(x =>
            x.Id == line1Id);

        var line2 = invoice.Lines.First(x => 
            x.Id == line2Id);

        _contextMock.Setup(x=>
            x.Invoice).Returns(invoice);

        // ***** ACT *****

        await sut.HydrateAsync(dto);

        // ***** ASSERT *****
        
        // One item per line
        Assert.Equal(2, dto.InvoiceItemDetails.Length);

        Assert.Contains(dto.InvoiceItemDetails, x =>
            x.Amount == line1.Amount
            && x.QuantityOrdered == line1.Quantity
            && x.LineId == line1Id
            && x.ProductName == line1.ProductName);
        
        Assert.Contains(dto.InvoiceItemDetails, x =>
            x.Amount == line2.Amount
            && x.QuantityOrdered == line2.Quantity
            && x.LineId == line2Id
            && x.ProductName == line2.ProductName);
    }

    class Dto : IInvoiceItemDetailsRootData
    {
        public Guid InvoiceId { get; set; }
        public InvoiceItemDetail[] InvoiceItemDetails { get; set; }
    }
}