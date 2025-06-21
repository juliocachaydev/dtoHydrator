using Example_EfCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example_EfCore.Infrastructure.Configurations;

public static class InvoiceTypeConfigurations
{
    class InvoiceTypeConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();
        }
    }
    
    class InvoiceLineTypeConfiguration : IEntityTypeConfiguration<Invoice.InvoiceLine>
    {
        public void Configure(EntityTypeBuilder<Invoice.InvoiceLine> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();
        }
    }
}