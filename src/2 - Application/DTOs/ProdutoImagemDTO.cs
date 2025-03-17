using System.ComponentModel.DataAnnotations;

namespace MyApiV8.Application.DTOs;

// Binder personalizado para envio de IFormFile e ViewModel dentro de um FormData compatível com .NET Core 3.1 ou superior (system.text.json)
//[ModelBinder(BinderType = typeof(ProdutoModelBinder))]
public class ProdutoImagemDTO
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

    // Evita o erro de conversão de string vazia para IFormFile
    //[JsonIgnore]
    //public IFormFile ImagemUpload { get; set; }

    //public string Imagem { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public decimal Value { get; set; }

    //[ScaffoldColumn(false)]
    public DateTime CreatedAt { get; set; }

    public bool Active { get; set; }

    //[ScaffoldColumn(false)]
    public string SupplierName { get; set; }
}