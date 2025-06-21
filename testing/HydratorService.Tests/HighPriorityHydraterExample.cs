using Hydrator.Api;

namespace HydratorService.Tests;

public static class HighPriorityHydraterExample
{
    public interface IHighPriorityExample : IHighPriorityHydratableValue
    {
        decimal Dividend { get; set; }

        decimal Divisor { get; set; }
    }

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

public static class LowPriorityHydraterExample
{
    public interface ILowPriorityExample : IHydratableValue
    {
        decimal Dividend { get; }

        decimal Divisor { get; }

        decimal Quotient { get; set; }
    }

    public class Handler : IHydratorHandler<ILowPriorityExample>
    {
        public Task HydrateAsync(ILowPriorityExample dto)
        {
            // Divisor and Dividend has values set before by the high priority handler: HighPriorityHydraterExample.Handler
            dto.Quotient = dto.Divisor / dto.Dividend;

            return Task.CompletedTask;
        }
    }
}