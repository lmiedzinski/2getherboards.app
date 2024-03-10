using TogetherBoardsApp.Backend.Application.Abstractions.Cryptography;

namespace TogetherBoardsApp.Backend.Infrastructure.Cryptography;

internal sealed class CryptographyService : ICryptographyService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool IsPasswordMatchingHash(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}