using Hydrator.Api;

namespace HydratorService.Tests;

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