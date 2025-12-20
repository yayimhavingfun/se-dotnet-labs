namespace Infrastructure.Dto;

public class AccountDto
{
    public Guid Id { get; set; }

    public required string AccountNumber { get; init; } = string.Empty;

    public required string PinHash { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public DateTime CreatedAt { get; set; }
}