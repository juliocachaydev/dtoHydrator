using System.Reflection;
using Example_EfCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace Example_EfCore.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Shipment> Shipments { get; set; }
    
    public DbSet<Customer> Customers { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}