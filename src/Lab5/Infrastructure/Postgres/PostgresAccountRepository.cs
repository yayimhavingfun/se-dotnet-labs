using Core.Application.Abstractions.Repositories;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;
using Dapper;
using Infrastructure.Dto;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Postgres;

public class PostgresAccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public PostgresAccountRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
                            ?? throw new InvalidOperationException("Postgres connection string not found");
    }

    public async Task<Account?> FindByNumberAsync(AccountNumber number, CancellationToken ct)
    {
        const string sql = """
                           SELECT 
                               id AS Id,
                               account_number AS AccountNumber,
                               pin_hash AS PinHash,
                               balance AS Balance,
                               created_at AS CreatedAt
                           FROM accounts
                           WHERE account_number = @AccountNumber
                           LIMIT 1
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        AccountDto? dto =
            await connection.QuerySingleOrDefaultAsync<AccountDto>(sql, new { AccountNumber = number.Value });

        return dto is null ? null : MapToAccount(dto);
    }

    public async Task<Account?> FindByIdAsync(Guid accountId, CancellationToken ct)
    {
        const string sql = """
                           SELECT 
                               id AS Id,
                               account_number AS AccountNumber,
                               pin_hash AS PinHash,
                               balance AS Balance,
                               created_at AS CreatedAt
                           FROM accounts
                           WHERE id = @Id
                           LIMIT 1
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        AccountDto? dto = await connection.QuerySingleOrDefaultAsync<AccountDto>(sql, new { Id = accountId });

        return dto is null ? null : MapToAccount(dto);
    }

    public async Task AddAsync(Account account, CancellationToken ct)
    {
        const string sql = """
                           INSERT INTO accounts (id, account_number, pin_hash, balance, created_at)
                           VALUES (@Id, @AccountNumber, @PinHash, @Balance, @CreatedAt)
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(
            sql,
            new
            {
                account.Id,
                AccountNumber = account.AccountNumber.Value,
                account.PinHash,
                Balance = account.Balance.Amount,
                account.CreatedAt,
            });
    }

    public async Task UpdateAsync(Account account, CancellationToken ct)
    {
        const string sql = """
                           UPDATE accounts
                           SET balance = @Balance
                           WHERE id = @Id
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(
            sql,
            new
            {
                account.Id,
                Balance = account.Balance.Amount,
            });
    }

    public async Task DeleteAsync(Guid accountId, CancellationToken ct)
    {
        const string sql = "DELETE FROM accounts WHERE id = @Id";

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(sql, new { Id = accountId });
    }

    private static Account MapToAccount(AccountDto dto)
    {
        AccountNumber accountNumber;
        Money balance;

        try
        {
            accountNumber = new AccountNumber(dto.AccountNumber);
            balance = new Money(dto.Balance);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException(
                $"Invalid data in database for account {dto.Id}: {ex.Message}",
                ex);
        }

        return Account.FromStorage(
            dto.Id,
            accountNumber,
            dto.PinHash,
            balance,
            dto.CreatedAt);
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}