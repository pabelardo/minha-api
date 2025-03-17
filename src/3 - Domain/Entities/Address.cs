namespace MyApiV8.Domain.Entities;

public class Address : Entity
{
    public Guid SupplierId { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Number { get; set; }
    public string ZipCode { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }

    /* EF Relation */
    public Supplier Supplier { get; set; }
}
