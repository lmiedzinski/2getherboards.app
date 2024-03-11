namespace TogetherBoardsApp.Backend.Infrastructure.Token;

public sealed class TokenOptions
{
    public const string SectionName = "TokenOptions";
    public AccessTokenOptions AccessTokenOptions { get; init; } = null!;
    public RefreshTokenOptions RefreshTokenOptions { get; init; } = null!;
}

public sealed class AccessTokenOptions
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string PrivateRsaCertificate { get; init; } = null!;
    public string PublicRsaCertificate { get; init; } = null!;
    public int LifeTimeInSeconds { get; init; }
}

public sealed class RefreshTokenOptions
{
    public int Length { get; init; }
    public int LifeTimeInMinutes { get; init; }
}