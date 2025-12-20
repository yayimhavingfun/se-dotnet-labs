using Core.Application.Abstractions.Repositories;
using Core.Domain.Entities;
using Core.Domain.Entities.Types;
using Core.Domain.ValueObjects;
using Dapper;
using Infrastructure.Dto;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Postgres;

public class PostgresTransactionRepository : ITransactionRepository
{
    private readonly string _connectionString;

    public PostgresTransactionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
                            ?? throw new InvalidOperationException("Postgres connection string not found");
    }

    public async Task AddAsync(Transaction transaction, CancellationToken ct)
    {
        const string sql = """
                           INSERT INTO transactions (id, account_id, type, amount, balance_after, created_at)
                           VALUES (@Id, @AccountId, @Type, @TransactionAmount, @NewBalance, @CreatedAt)
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
                NewBalance = transaction.NewBalance.Amount,
                transaction.CreatedAt,
            });
    }

    public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken ct)
    {
        const string sql = """
                           SELECT id, account_id, type, amount, balance_after, created_at
                           FROM transactions
                           WHERE account_id = @AccountId
                           ORDER BY created_at DESC
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        IEnumerable<TransactionDto> dtos =
            await connection.QueryAsync<TransactionDto>(sql, new { AccountId = accountId }, commandTimeout: 30);

        var transactions = new List<Transaction>();
        foreach (TransactionDto dto in dtos)
        {
            transactions.Add(new Transaction(
                dto.Id,
                dto.AccountId,
                type: Enum.Parse<TransactionType>(dto.Type),
                new Money(dto.Amount),
                new Money(dto.NewBalance),
                dto.CreatedAt));
        }

        return transactions;
    }

    public async Task DeleteByAccountIdAsync(Guid accountId, CancellationToken ct)
    {
        const string sql = "DELETE FROM transactions WHERE account_id = @AccountId";

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(sql, new { AccountId = accountId });
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}