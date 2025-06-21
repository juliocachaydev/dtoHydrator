# Dto Hydrator

## Overview
A C# library that hydrates a DTO using the Separation of Concerns (SoC) principle, 
where each handler is responsible for hydrating a specific part of the DTO. Multiple handlers 
can contribute to the hydration process, ensuring modular and maintainable code.

## Problem to solve
In many applications, data transfer objects (DTOs) and view-models (VMs) are build from various sources (multiple entities) and sometimes, some properties are
calculated values.

This library allows you to define interfaces for each part of the DTO that needs to be hydrated, and then implement handlers that will populate those parts. Each
handler can receive dependencies through dependency injection, making it easy to manage complex hydration logic.

## Dependencies

- .NET Standard 2.1

## Features

- Handler-based hydration
- Dependency injection support

## Getting Started
> TODO: Add nuget here...

## Usage
### Add the hydrator library via dependency injection

You can pass one or more assemblies. The library will scan the assemblies for hydrator handlers.
```csharp
services.AddHydrator(Assembly.GetExecutingAssembly());
```

### Define a Hydratable value and its handler
```csharp

public  interface ICustomerName : IHydratableValue
{
    public Guid CustomerId {get;}
    
    public string CustomerName {get; set;}
    
    class Handler : IHydratorHandler<CustomerName>
    {
        private readonly ICacheService _cacheService;
        // You can inject dependencies in the constructor, provided by the DI container
        public Handler(ICacheService cacheService)
        {
            _cacheService  = cacheService; 
        }
        public Task HydrateAsync(CustomerName dto)
        {
            var name = _cacheService.Customers.First(x=> x.Id == dto.CustomerId).Name;   
            dto.CustomerName = name;
        }
    }
}

```

### Implement your HydratableValue interfaces

```csharp
public class InvoiceViewModel : ICustomerName
{
     public Guid InvoiceId {get; set;}
     
     public Guid CustomerId {get; set;}
     
     public string CustomerName {get; set;} // <-- hydrated value
}
```

### Use the service
```csharp
public class InvoiceQuery
{
    private readonly IHydratorService _hydrator;
    private readonly IInvoiceRepository _invoiceRepository;
    
    public InvoiceQuery(IHydratorService hydrator, IInvoiceRepository invoiceRepository)
    {
        _hydrator = hydrator;
        _invoiceRepository = invoiceRepository;
    }
       
    public InvoiceViewModel GetInvoice(Guid invoiceId)
    {
        var invoice = _invoiceRepository.GetById(invoiceId);
        var viewModel = new InvoiceViewModel
        {
            InvoiceId = invoice.Id,
            CustomerId = invoice.CustomerId
        };
        
        // Hydrate the view model
        _hydrator.HydrateAsync(viewModel).GetAwaiter().GetResult();
        
        return viewModel;
    }
}
```

## Important Considerations
The Hydrator Service is registered with a scoped lifetime, meaning a new instance is created for each request.

A typical pattern is to use a scoped cache service to load all necessary data up front. This cache can then be injected into each hydrator 
handler, enabling efficient data access and reducing repeated database queries.

## Use Cases

### Avoid Duplicating Logic
Often, the same value needs to be mapped in multiple locations. For example, an entity’s name, such as a customer name, might be required in invoices, 
orders, statements, and more.

By using this library, you can implement a single handler to hydrate the customer name and reuse it wherever needed, eliminating redundant logic.

### Allow complex ViewModel building
For complex view-models with multiple nested DTOs or advanced calculations, this library enables you to break down the hydration process into several 
handlers. Each handler manages a specific aspect of the view-model, promoting modularity and easier maintenance.

## Examples
An example demonstrating a complex view-model hydrated with EF Core and SQLite is available in the repository.

## Open Source & License

This project is open source, licensed under the [MIT License](LICENSE).

Created by Julio Cachay G. in Chattanooga, TN, USA.

## Contributing

Contributions are welcome! If you would like to report a bug, suggest a feature, or submit a pull request, please follow these steps:

1. Fork the repository.
2. Create a new branch for your changes.
3. Make your changes and commit them with clear messages.
4. Open a pull request describing your changes.

For major changes, please open an issue first to discuss what you would like to change.

By contributing, you agree that your contributions will be licensed under the MIT License.

