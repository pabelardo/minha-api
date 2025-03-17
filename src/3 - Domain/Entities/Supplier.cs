using MyApiV8.Domain.Enums;

namespace MyApiV8.Domain.Entities;

public class Supplier : Entity
{
    public string Name { get; set; }
    public string Document { get; set; }
    public SupplierTypeEnum SupplierType { get; set; }
    public Address Address { get; set; }
    public bool Active { get; set; }

    /* EF Relations */
    public IEnumerable<Product> Products { get; set; }
}
