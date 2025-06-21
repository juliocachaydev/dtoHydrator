using Example_EfCore.Features.Sales;
using Example_EfCore.Features.Sales.InvoiceOverviews;
using Microsoft.AspNetCore.Mvc;

namespace Example_EfCore.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceOverviewQuery _invoiceOverviewQuery;

    public InvoicesController(
        IInvoiceOverviewQuery invoiceOverviewQuery)
    {
        _invoiceOverviewQuery = invoiceOverviewQuery;
    }
    

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceOverviewViewModel[]>>> GetInvoiceOverviews()
    {
        var result = await _invoiceOverviewQuery.GetInvoiceDetails();
        
        return Ok(result);
    }
}