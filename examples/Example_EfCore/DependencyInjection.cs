using System.Reflection;
using Example_EfCore.Features.Sales;
using Example_EfCore.Features.Sales.InvoiceList;
using Example_EfCore.Features.Sales.InvoiceOverviews;
using Example_EfCore.Infrastructure;
using Hydrator.Api;
using Microsoft.EntityFrameworkCore;

namespace Example_EfCore;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=localdb.sqlite"));

        services.AddScoped<IMockDataService, IMockDataService.Imp>();

        services.AddScoped<IInvoiceOverviewQuery, IInvoiceOverviewQuery.Imp>();

        services.AddScoped<IInvoiceOverviewsContext, IInvoiceOverviewsContext.Imp>();

        services.AddScoped<IInvoiceListQuery, IInvoiceListQuery.Imp>();
        
        services.AddHydrator(Assembly.GetExecutingAssembly());
    }
}