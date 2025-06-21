namespace HydratorService.Tests;

public class DtoExample : HighPriorityHydraterExample.IHighPriorityExample,
    LowPriorityHydraterExample.ILowPriorityExample
{
    public decimal Dividend { get; set; }
    public decimal Divisor { get; set; }
    public decimal Quotient { get; set; }
}