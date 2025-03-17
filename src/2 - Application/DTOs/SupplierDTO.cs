using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyApiV8.Application.DTOs;

public class SupplierDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("document")]
    public string Document { get; set; }

    [JsonPropertyName("sup_type")]
    public int SupplierType { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO Address { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("products")]
    public IEnumerable<ProductDTO> Products { get; set; }
}