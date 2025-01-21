using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTOs;

public class TransacoesCreateResponseDTO
{    
    public string? Id { get; set; }

    public string? Txid { get; set; }

    public decimal Valor { get; set; }

    public DateTime Data_Transacao { get; set; }
}
