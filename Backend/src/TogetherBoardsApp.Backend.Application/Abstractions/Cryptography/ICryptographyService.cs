namespace TogetherBoardsApp.Backend.Application.Abstractions.Cryptography;

public interface ICryptographyService
{
    string HashPassword(string password);
    bool IsPasswordMatchingHash(string password, string passwordHash);
}