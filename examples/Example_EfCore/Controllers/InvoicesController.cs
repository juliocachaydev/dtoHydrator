using Example_EfCore.Features.Sales;
using Example_EfCore.Features.Sales.InvoiceList;
using Example_EfCore.Features.Sales.InvoiceOverviews;
using Example_EfCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Example_EfCore.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceOverviewQuery _invoiceOverviewQuery;
    private readonly IInvoiceListQuery _invoiceListQuery;

    public InvoicesController(
        IInvoiceOverviewQuery invoiceOverviewQuery,
        IInvoiceListQuery invoiceListQuery)
    {
        _invoiceOverviewQuery = invoiceOverviewQuery;
        _invoiceListQuery = invoiceListQuery;
    }

    [HttpGet]
    public async Task<ActionResult<InvoiceListViewModel>> GetInvoiceList()
    {
        var result = await _invoiceListQuery.GetInvoiceListAsync();

        return Ok(result);
    }
    

    [HttpGet("overview-for-first-invoice")]
    public async Task<ActionResult<InvoiceOverviewViewModel>> GetInvoiceOverviews(
        [FromServices] AppDbContext db)
    {
        /*
         * In a real case, this endpoint would accept an invoice ID as a parameter, but for an example it is easier to just use the first invoice in the database.
         */
        var invoiceId = db.Invoices.First().Id;
        
        var result = await _invoiceOverviewQuery.GetInvoiceDetails(invoiceId);
        
        return Ok(result);
    }
}