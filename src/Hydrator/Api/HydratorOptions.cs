namespace Hydrator.Api
{
    public class HydratorOptions
    {
        public bool AssertEachHydratableValueHasHandler { get; set; } = true;

        public bool EnsureAllDependenciesAreRegistered { get; set; } = true;
    }
}