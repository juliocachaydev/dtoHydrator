# Dto Hydrator

More information on the [Wiki](https://github.com/juliocachaydev/dtoHydrator/wiki/1.-Home)

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
> dotnet add package DtoHydrator --version 1.0.0

## Usage
### Setup the DI Container
```csharp
services.AddHydrator(Assembly.GetExecutingAssembly());
```

### Example: Use case

We will define a DTO example that has 3 properties: Dividend, Divisor and Quotient.

Then, we will use the library to perform two operations:
1. Set the Dividend And Divisor
2. Calculate the Quotient.

This is an example intended to demonstrate how the library works.

### Add a Hydrator that sets the Dividend and Divisor
**Important:**
1. Following the Interface Segregation Principle (the I in SOLID), the hydratable value only exposes the members needed to fulfill its purpose.
2. Using `IHighPriorityHydratableValue` ensures this value is hydrated first.
3. Nesting the `Handler` class is optional and provided here for convenience.

**You can inject any scoped service in the Handler, which allows you to perform operations against a database, a cache or other services for more complex use cases**

```csharp
public interface IHighPriorityExample : IHighPriorityHydratableValue
{
    decimal Dividend { get; set; }

    decimal Divisor { get; set; }
        
    public class Handler : IHydratorHandler<IHighPriorityExample>
    {
        public Task HydrateAsync(IHighPriorityExample dto)
        {
            dto.Dividend = 100;
            dto.Divisor = 10;
            return Task.CompletedTask;
        }
    }
}

```

### Add a Hydrator that calculates the Quotient
**Important:**
1. Notice that `Dividend` and `Divisor` are read-only properties here, meaning this hydrator only reads them to perform its calculation.
2. The `Quotient` property has a setter, indicating that this hydrator is responsible for assigning its value.

```csharp
public interface ILowPriorityExample : IHydratableValue
{
    decimal Dividend { get; }

    decimal Divisor { get; }

    decimal Quotient { get; set; }
        
    public class Handler : IHydratorHandler<ILowPriorityExample>
    {
        public Task HydrateAsync(ILowPriorityExample dto)
        {
            // Divisor and Dividend has values set before by the high priority handler: HighPriorityHydraterExample.Handler
            dto.Quotient = dto.Dividend / dto.Divisor;

            return Task.CompletedTask;
        }
    }
}
```

### Use the Hydrators to tell the library which values should be populated

The code below indicates that the DtoExample is can be hydrated with two different Hydrators, as shown above,
the first one sets the Dividend and Divisor and the second one calculates the Quotient.

```csharp
public class DtoExample : 
    IHighPriorityExample,
    ILowPriorityExample
{
    public decimal Dividend { get; set; }
    public decimal Divisor { get; set; }
    public decimal Quotient { get; set; }
}
```

### Use the IHydratorService
```csharp
 public class DemoUseCase
    {
        private readonly IHydratorService _hydrator;

        public DemoUseCase(IHydratorService hydrator)
        {
            _hydrator = hydrator;
        }
        
        public async Task<DtoExample> GetDtoAsync()
        {
            var dto = new DtoExample();
            await _hydrator.HydrateAsync(dto);
            return dto;
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

