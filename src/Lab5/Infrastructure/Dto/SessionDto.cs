namespace Infrastructure.Dto;

public class SessionDto
{
    public Guid SessionId { get; set; }

    public bool IsAdmin { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? AccountId { get; set; }

    public string? AccountNumber { get; set; }

    public string? PinHash { get; set; }

    public decimal Balance { get; set; }

    public DateTime AccountCreatedAt { get; set; }
}