using Hydrator.Api;

namespace HydratorService.Tests;

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