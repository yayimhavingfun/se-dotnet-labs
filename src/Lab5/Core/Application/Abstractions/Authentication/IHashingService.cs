namespace Core.Application.Abstractions.Authentication;

public interface IHashingService
{
    string Hash(string plainText);

    bool Verify(string plainText, string hashWithSalt);
}