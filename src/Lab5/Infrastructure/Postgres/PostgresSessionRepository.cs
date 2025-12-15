using Dapper;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;
using Npgsql;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Postgres;

public class PostgresSessionRepository : ISessionRepository
{
    private readonly string _connectionString;

    public PostgresSessionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
                            ?? throw new InvalidOperationException("Postgres connection string not found");
    }

    public async Task<AtmSession?> FindByIdAsync(Guid sessionId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT 
                               s.session_id,
                               s.is_admin,
                               s.created_at,
                               a.id AS account_id,
                               a.account_number,
                               a.pin_hash,
                               a.balance,
                               a.created_at AS account_created_at
                           FROM sessions s
                           LEFT JOIN accounts a ON s.user_account_id = a.id
                           WHERE s.session_id = @SessionId
                           LIMIT 1
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        dynamic? row = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { SessionId = sessionId });

        return row is null ? null : MapToSession(row);
    }

    public async Task AddAsync(AtmSession atmSession, CancellationToken ct = default)
    {
        const string sql = """
                           INSERT INTO sessions (session_id, is_admin, user_account_id, created_at)
                           VALUES (@SessionId, @IsAdmin, @UserAccountId, @CreatedAt)
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(
            sql,
            new
            {
                atmSession.SessionId,
                atmSession.IsAdmin,
                UserAccountId = atmSession.Account?.Id,
                atmSession.CreatedAt,
            });
    }

    public async Task DeleteAsync(Guid sessionId, CancellationToken ct = default)
    {
        const string sql = "DELETE FROM sessions WHERE session_id = @SessionId";

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(sql, new { SessionId = sessionId });
    }

    private static AtmSession MapToSession(dynamic row)
    {
        if (row.is_admin)
        {
            return AtmSession.CreateForAdmin();
        }

        var account = Account.FromStorage(
            id: (Guid)row.account_id,
            number: new AccountNumber((string)row.account_number),
            pinHash: (string)row.pin_hash,
            balance: new Money((decimal)row.balance),
            createdAt: (DateTime)row.account_created_at);

        return AtmSession.CreateForUser(account);
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}