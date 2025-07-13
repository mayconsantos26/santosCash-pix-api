namespace DTOs;

public class TransacoesCreateResponseDTO
{    
    public int? Id { get; set; }

    public string? Txid { get; set; }

    public decimal Valor { get; set; }
    
    public DateTime Data_Transacao { get; set; } = DateTime.UtcNow;
}
