using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTOs;
public class TransacoesUpdateDTO
{
    [Column("txid")]
    [StringLength(35, MinimumLength = 26, ErrorMessage = "O Txid deve ter entre 26 e 35 caracteres.")]
    [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "O Txid deve ter apenas letras e números.")]
    public string? Txid { get; set; } // Identificador único da transação deve ter entre 26 a 35 caracteres

    // Dados pagador (quem paga)
    [StringLength(100)]
    [Column("pagador_nome")]
    public string? Pagador_Nome { get; set; } // Nome do pagador

    [Column("pagador_documento")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "O documento do pagador deve ter entre 11 (CPF) e 14 (CNPJ) dígitos.")]
    public string? Pagador_Documento { get; set; }

    [Column("pagador_banco")]
    [StringLength(8)]
    public string? Pagador_Banco { get; set; } // Código ISPB do banco do pagador

    [Column("pagador_agencia")]
    [StringLength(6)]
    public string? Pagador_Agencia { get; set; } // Agência do pagador

    [Column("pagador_conta")]
    [StringLength(10)]
    public string? Pagador_Conta { get; set; } // Conta do pagador

    // Dados recebedor (quem recebe)
    [Column("recebedor_nome")]
    [StringLength(100)]
    public string? Recebedor_Nome { get; set; } // Nome do recebedor

    [Column("recebedor_documento")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "O documento do pagador deve ter entre 11 (CPF) e 14 (CNPJ) dígitos.")]
    public string? Recebedor_Documento { get; set; }

    [Column("recebedor_banco")]
    [StringLength(8)]
    public string? Recebedor_Banco { get; set; } // Código ISPB do banco do recebedor

    [Column("recebedor_agencia")]
    [StringLength(6)]
    public string? Recebedor_Agencia { get; set; } // Agência do recebedor

    [Column("recebedor_conta")]
    [StringLength(10)]
    public string? Recebedor_Conta { get; set; } // Conta do recebedor
}


