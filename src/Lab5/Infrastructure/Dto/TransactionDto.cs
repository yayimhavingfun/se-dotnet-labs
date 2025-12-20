namespace Infrastructure.Dto;

public class TransactionDto
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public decimal NewBalance { get; set; }

    public DateTime CreatedAt { get; set; }
}