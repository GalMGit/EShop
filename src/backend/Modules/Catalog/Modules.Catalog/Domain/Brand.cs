namespace Modules.Catalog.Domain;

public sealed class Brand
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }
}