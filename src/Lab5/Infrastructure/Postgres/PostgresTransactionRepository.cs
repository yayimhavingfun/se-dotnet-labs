using Dapper;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities.Types;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;
using Npgsql;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Postgres;

public class PostgresTransactionRepository : ITransactionRepository
{
    private readonly string _connectionString;

    public PostgresTransactionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
                            ?? throw new InvalidOperationException("Postgres connection string not found");
    }

    public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
    {
        const string sql = """
                           INSERT INTO transactions (id, account_id, type, amount, balance_after, created_at)
                           VALUES (@Id, @AccountId, @Type, @TransactionAmount, @BalanceAfter, @CreatedAt)
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(
            sql,
            new
            {
                transaction.Id,
                transaction.AccountId,
                Type = transaction.Type.ToString(),
                TransactionAmount = transaction.Amount.Amount,
                BalanceAfter = transaction.NewBalance.Amount,
                transaction.CreatedAt,
            });
    }

    public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT id, account_id, type, amount, balance_after, created_at
                           FROM transactions
                           WHERE account_id = @AccountId
                           ORDER BY created_at DESC
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        IEnumerable<dynamic> rows =
            await connection.QueryAsync<dynamic>(sql, new { AccountId = accountId }, commandTimeout: 30);

        var transactions = new List<Transaction>();
        foreach (dynamic row in rows)
        {
            transactions.Add(new Transaction(
                row.id,
                row.account_id,
                Enum.Parse<TransactionType>(row.type),
                new Money(row.amount),
                new Money(row.balance_after),
                row.created_at));
        }

        return transactions;
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}