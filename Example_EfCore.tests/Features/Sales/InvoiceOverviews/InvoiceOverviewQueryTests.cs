using Example_EfCore.Domain;
using Example_EfCore.Features.Sales;
using Example_EfCore.Features.Sales.InvoiceOverviews;
using Hydrator.Api;
using Hydrator.Utils;
using Moq;

namespace Example_EfCore.tests.Features.Sales;

public class InvoiceOverviewQueryTests
{
    private readonly Mock<IInvoiceOverviewsContext> _contextMock;

    private readonly IHydratorServiceMock _hydratorServiceMock;

    public InvoiceOverviewQueryTests()
    {
        _contextMock = new();
        _contextMock.Setup(x => x.Invoice)
            .Returns(new Invoice(Guid.NewGuid()));
        
        // Make sure collections are not null
        _contextMock.Setup(x => x.Payments)
            .Returns([]);
        _contextMock.Setup(x => x.Shipments)
            .Returns([]);
        
        _contextMock.Setup(x=> x.Result)
            .Returns(new InvoiceOverviewViewModel()
            {
                InvoiceId = Guid.NewGuid(),
                InvoiceItemDetails = [],
                Payments = []
            });
        
        _hydratorServiceMock = new();
    }

    private IInvoiceOverviewQuery CreateSut()
    {
        return new IInvoiceOverviewQuery.Imp(_contextMock.Object, _hydratorServiceMock.Object);
    }
    
    [Fact]
    public async Task PreloadsDataForSpecifiedInvoice_CreatesAndHydratesAViewModel_ReturnsViewModelForLoadedInvoice()
    {
        // ***** ARRANGE *****
        
        var sut = CreateSut();

        var invoiceId = Guid.NewGuid();

        // The loader has one invoice pre-loaded
        _contextMock.Setup(x =>
            x.Invoice).Returns(new Invoice(invoiceId));

        // ***** ACT *****
        
        var result = await sut.GetInvoiceDetails(invoiceId);

        // ***** ASSERT *****
        
        // Preloads data
        _contextMock.Verify(x=> x.LoadAsync(invoiceId));
        
        // Hydrates the view-model, there are multiple hydrators implemented by the view-model but that is a concern of the hydrator service
        Assert.True(_hydratorServiceMock.WasCalled(result));
    }

    
    [Fact]
    public async Task HydratesEachInvoiceItemDetail()
    {
        // ***** ARRANGE *****
        
        var sut = CreateSut();

        var invoiceId = Guid.NewGuid();

        // The loader has one invoice pre-loaded
        _contextMock.Setup(x =>
            x.Invoice).Returns(new Invoice(invoiceId));

        var itemDetail1 = new InvoiceItemDetail
        {
            LineId = Guid.NewGuid(),
            ProductName = "Bolts",
            UnitPrice = 100,
            Amount = 1,
            QuantityOrdered = 1000,
            QuantityShipped = 0
        };
        
        var itemDetail2 = new InvoiceItemDetail
        {
            LineId = Guid.NewGuid(),
            ProductName = "Bolts 2",
            UnitPrice = 100,
            Amount = 1,
            QuantityOrdered = 1000,
            QuantityShipped = 0
        };

        var viewModel = new InvoiceOverviewViewModel
        {
            InvoiceId = Guid.NewGuid(),
            InvoiceItemDetails = [itemDetail1, itemDetail2],
            Payments = []
        };
        
        _contextMock.Setup(x=> x.Result)
            .Returns(viewModel);

        // ***** ACT *****
        
       await sut.GetInvoiceDetails(invoiceId);

        // ***** ASSERT *****
       
        // Each item detail is hydrated to set the Quantity Shipped property
        Assert.True(_hydratorServiceMock.WasCalled(itemDetail1));
        Assert.True(_hydratorServiceMock.WasCalled(itemDetail2));
    }
    
}