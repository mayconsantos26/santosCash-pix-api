using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTOs;
public class TransacoesCreateRequestDTO
{    
    [Required]
    [Column("valor", TypeName = "decimal(10,2)")]
    [Range(0.01 , double.MaxValue, ErrorMessage = "O valor da transação deve ser maior que zero.")]
    public decimal Valor { get; set; } // Valor da transação
}
