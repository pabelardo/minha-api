using System.ComponentModel.DataAnnotations;

namespace MyApiV8.Application.DTOs;

public class ProductDTO
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]

    public Guid SupplierId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Description { get; set; }

    public string ImageUpload { get; set; }

    public string Image { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public decimal Value { get; set; }

    [ScaffoldColumn(false)]
    public DateTime CreatedAt { get; set; }

    public bool Active { get; set; }

    [ScaffoldColumn(false)]
    public string SupplierName { get; set; }
}