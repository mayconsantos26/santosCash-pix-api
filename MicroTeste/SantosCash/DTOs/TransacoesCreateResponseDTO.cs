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
    public string? Txid { get; set; } // Identificador único da transação deve ter entre 26 a 35 caracteres 
    
    [Required]
    [Column("valor", TypeName = "decimal(10,2)")]
    public decimal Valor { get; set; } // Valor da transação

    [Required]
    [Column("data_transacao")]
    public DateTime Data_Transacao { get; set; } // Data e hora da transação
}
