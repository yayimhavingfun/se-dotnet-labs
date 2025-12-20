using Core.Domain.Entities.Types;
using Core.Domain.Results;
using Core.Domain.ValueObjects;

namespace Core.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }

    public AccountNumber AccountNumber { get; private set; }

    public string PinHash { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Money Balance { get; private set; }

    public Account(AccountNumber accountNumber, string pinHash, Money? initialBalance = null)
    {
        Id = Guid.NewGuid();
        AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
        PinHash = pinHash ?? throw new ArgumentNullException(nameof(pinHash));
        Balance = initialBalance ?? Money.Zero;
        CreatedAt = DateTime.UtcNow;
    }

    public static Account FromStorage(Guid id, AccountNumber number, string pinHash, Money balance, DateTime createdAt)
    {
        var account = new Account(number, pinHash, balance)
        {
            Id = id,
            CreatedAt = createdAt,
        };
        return account;
    }

    public TransactionResult Deposit(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Amount == 0)
            return new TransactionResult.Failure("INVALID_AMOUNT", "Deposit amount cannot be zero");

        Balance += amount;

        var transaction = new Transaction(
            Guid.NewGuid(),
            Id,
            TransactionType.Deposit,
            amount,
            Balance,
            DateTime.UtcNow);

        return new TransactionResult.Success(transaction);
    }

    public TransactionResult Withdraw(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Amount == 0)
            return new TransactionResult.Failure("INVALID_AMOUNT", "Deposit amount cannot be zero");

        if (Balance.IsLessThan(amount))
            return new TransactionResult.Failure("INSUFFICIENT_FUNDS", "Insufficient funds for the operation");

        Balance -= amount;

        var transaction = new Transaction(
            Guid.NewGuid(),
            Id,
            TransactionType.Withdrawal,
            amount,
            Balance,
            DateTime.UtcNow);

        return new TransactionResult.Success(transaction);
    }

    public BalanceResult ViewBalance()
    {
        return new BalanceResult.Success(Balance);
    }
}