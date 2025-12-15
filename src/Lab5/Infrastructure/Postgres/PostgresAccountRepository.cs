using Dapper;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;
using Npgsql;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Postgres;

public class PostgresAccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public PostgresAccountRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
                            ?? throw new InvalidOperationException("Postgres connection string not found");
    }

    public async Task<Account?> FindByNumberAsync(AccountNumber number, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT id, account_number, pin_hash, balance, created_at
                           FROM accounts
                           WHERE account_number = @AccountNumber
                           LIMIT 1
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        dynamic? row = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { AccountNumber = number.Value });

        return row is null ? null : MapToAccount(row);
    }

    public async Task<Account?> FindByIdAsync(Guid accountId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT id, account_number, pin_hash, balance, created_at
                           FROM accounts
                           WHERE id = @Id
                           LIMIT 1
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        dynamic? row = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Id = accountId });

        return row is null ? null : MapToAccount(row);
    }

    public async Task AddAsync(Account account, CancellationToken ct = default)
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

    public async Task UpdateAsync(Account account, CancellationToken ct = default)
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

    private static Account MapToAccount(dynamic row)
    {
        return Account.FromStorage(
            id: row.id,
            number: new AccountNumber(row.account_number),
            pinHash: row.pin_hash,
            balance: new Money(row.balance),
            createdAt: row.created_at);
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}