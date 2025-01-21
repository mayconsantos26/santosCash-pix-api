using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTOs;

public class TransacoesCreateResponseDTO
{    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Gera o ID automaticamente no banco
    public string? Id { get; set; }

    [Required]
    [Column("txid")]
    [StringLength(35, MinimumLength = 26, ErrorMessage = "O Txid deve ter entre 26 e 35 caracteres.")]
    [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "O Txid deve ter apenas letras e números.")]
    public string? Txid { get; set; } // Identificador único da transação deve ter entre 26 a 35 caracteres 
    
    [Required]
    [Column("valor", TypeName = "decimal(10,2)")]
    [Range(0.01 , double.MaxValue, ErrorMessage = "O valor da transação deve ser maior que zero.")]
    public decimal Valor { get; set; } // Valor da transação

    [Required]
    [Column("data_transacao")]
    public DateTime Data_Transacao { get; set; } // Data e hora da transação
}
