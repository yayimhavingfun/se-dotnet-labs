using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Authentication;

public class HashingService : IHashingService
{
    private const int SaltSize = 32;

    public string Hash(string plainText)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
        string salt = Convert.ToBase64String(saltBytes);

        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainText + salt));
        string hash = Convert.ToBase64String(hashBytes);

        return $"{hash}:{salt}";
    }

    public bool Verify(string plainText, string hashWithSalt)
    {
        if (string.IsNullOrWhiteSpace(plainText) || string.IsNullOrWhiteSpace(hashWithSalt))
            return false;

        string[] parts = hashWithSalt.Split(':', 2);
        if (parts.Length != 2) return false;

        string hash = parts[0];
        string salt = parts[1];

        byte[] computedHashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainText + salt));
        string computedHash = Convert.ToBase64String(computedHashBytes);

        return hash == computedHash;
    }
}