using Core.Application.Abstractions.Repositories;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;
using Dapper;
using Infrastructure.Dto;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Postgres;

public class PostgresSessionRepository : ISessionRepository
{
    private readonly string _connectionString;

    public PostgresSessionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
                            ?? throw new InvalidOperationException("Postgres connection string not found");
    }

    public async Task<AtmSession?> FindByIdAsync(Guid sessionId, CancellationToken ct)
    {
        const string sql = """
                           SELECT 
                               s.session_id AS SessionId,
                               s.is_admin AS IsAdmin,
                               s.created_at AS CreatedAt,
                               a.id AS AccountId,
                               a.account_number AS AccountNumber,
                               a.pin_hash AS PinHash,
                               a.balance AS Balance,
                               a.created_at AS AccountCreatedAt
                           FROM sessions s
                           LEFT JOIN accounts a ON s.user_account_id = a.id
                           WHERE s.session_id = @SessionId
                           LIMIT 1
                           """;

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        SessionDto? dto = await connection.QuerySingleOrDefaultAsync<SessionDto>(sql, new { SessionId = sessionId });

        return dto is null ? null : MapToSession(dto);
    }

    public async Task AddAsync(AtmSession atmSession, CancellationToken ct)
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

    public async Task DeleteAsync(Guid sessionId, CancellationToken ct)
    {
        const string sql = "DELETE FROM sessions WHERE session_id = @SessionId";

        await using NpgsqlConnection connection = await OpenConnectionAsync(ct);
        await connection.ExecuteAsync(sql, new { SessionId = sessionId });
    }

    private static AtmSession MapToSession(SessionDto dto)
    {
        if (dto.IsAdmin || dto.AccountId is null)
        {
            return AtmSession.CreateForAdmin();
        }

        var account = Account.FromStorage(
            id: dto.AccountId.Value,
            number: new AccountNumber(dto.AccountNumber
                                      ?? throw new InvalidOperationException("AccountNumber is null for user session")),
            pinHash: dto.PinHash ?? throw new InvalidOperationException("PinHash is null for user session"),
            balance: new Money(dto.Balance),
            createdAt: dto.AccountCreatedAt);

        return AtmSession.CreateForUser(account);
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}