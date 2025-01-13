using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroTeste.Models;

[Table("keys")]
public class Keys
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; } // Chave Primária

    [Column("key")]
    [StringLength(64)]
    public string? ApiKey { get; set; } // Chave de API única usada para autenticação

    [Column("nome")]
    [StringLength(100)]
    public string? Nome { get; set; } // Nome da entidade (empresa ou organização) associada à chave

    [Column("cnpj")]
    [StringLength(14, ErrorMessage = "O CNPJ deve ter exatamente 14 dígitos.")]
    public string? Cnpj { get; set; } // O documento entidade 

    [Column("conta")]
    [StringLength(10)]
    public string? Conta { get; set; } // Número da conta bancária da entidade
}

