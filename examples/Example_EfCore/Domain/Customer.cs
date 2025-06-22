namespace Example_EfCore.Domain;

public class Customer
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    // EF Core
    private Customer()
    {
        
    }

    public Customer(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}