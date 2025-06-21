using Example_EfCore.Domain;
using Example_EfCore.Features.Sales.InvoiceOverviews;
using Moq;

namespace Example_EfCore.tests.Features.Sales.Hydrator;

public class IInvoiceItemDetailQuantityShippedTests
{
    private readonly Mock<IInvoiceOverviewsContext> _contextMock;

    public IInvoiceItemDetailQuantityShippedTests()
    {
        _contextMock = new Mock<IInvoiceOverviewsContext>();
    }

    private IInvoiceItemDetailQuantityShipped.Handler CreateSut()
    {
        return new(_contextMock.Object);
    }

    [Fact]
    public async Task SetsQuantityShipped()
    {
        // ***** ARRANGE *****

        // Instead of thinking about the view-model, we just worry about a subset of members.
        var dto = new Mock<IInvoiceItemDetailQuantityShipped>();
        
        // The Dto has a property for the product name set to bolts, this is what we will use to calculate the quantity shipped.
        dto.Setup(x=> x.ProductName)
            .Returns("Bolts");

        // There are two shipments of bolts, one with 100 and one with 200. Total 300.
        var shipment1 = new Shipment(Guid.NewGuid(), Guid.NewGuid(), "Bolts", 100);
        var shipment2 = new Shipment(Guid.NewGuid(), Guid.NewGuid(), "Bolts", 200);
        
        // This one should be excluded, because it is not for bolts.
        var shipment3 = new Shipment(Guid.NewGuid(), Guid.NewGuid(), "Nuts", 200);
        
        _contextMock.Setup(x=> x.Shipments)
            .Returns([shipment1, shipment2]);
        
        var sut = CreateSut();

        // ***** ACT *****

        await sut.HydrateAsync(dto.Object);

        // ***** ASSERT *****
        
        // The hydrater calculates the total quantity shipped for the product with name "Bolts" and sets the value in the Dto.
        dto.VerifySet(x=> x.QuantityShipped = 300);
    }
    
    
}