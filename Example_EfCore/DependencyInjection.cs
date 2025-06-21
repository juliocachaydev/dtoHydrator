using Example_EfCore.Features.Sales;
using Example_EfCore.Infrastructure;
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
    }
}