using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Domain.UserAccounts;

namespace TogetherBoardsApp.Backend.Infrastructure.Token;

internal sealed class TokenService : ITokenService
{
    private readonly TokenOptions _tokenOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly RsaKeysProvider _rsaKeysProvider;

    public TokenService(
        IOptions<TokenOptions> tokenServiceOptions,
        IHttpContextAccessor httpContextAccessor,
        IDateTimeProvider dateTimeProvider,
        RsaKeysProvider rsaKeysProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _dateTimeProvider = dateTimeProvider;
        _rsaKeysProvider = rsaKeysProvider;
        _tokenOptions = tokenServiceOptions.Value;
    }

    public string GenerateAccessToken(UserAccountId userAccountId)
    {
        var signingCredentials = new SigningCredentials(
            key: _rsaKeysProvider.RsaPrivateSecurityKey,
            algorithm: SecurityAlgorithms.RsaSha256
        );
        
        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userAccountId.Value.ToString()));
        
        var jwtHandler = new JwtSecurityTokenHandler();

        var jwt = jwtHandler.CreateJwtSecurityToken(
            issuer: _tokenOptions.AccessTokenOptions.Issuer,
            audience: _tokenOptions.AccessTokenOptions.Audience,
            subject: claimsIdentity,
            notBefore: _dateTimeProvider.UtcNow,
            expires: _dateTimeProvider.UtcNow.AddSeconds(_tokenOptions.AccessTokenOptions.LifeTimeInSeconds),
            issuedAt: _dateTimeProvider.UtcNow,
            signingCredentials: signingCredentials);

        return jwtHandler.WriteToken(jwt);
    }

    public string GenerateRefreshToken()
    {
        var length = _tokenOptions.RefreshTokenOptions.Length;
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
    }

    public int GetRefreshTokenLifetimeInMinutes()
    {
        return _tokenOptions.RefreshTokenOptions.LifeTimeInMinutes;
    }

    public UserAccountId GetUserAccountIdFromContext()
    {
        var userAccountIdValue = _httpContextAccessor
                .HttpContext?
                .User
                .Claims
                .SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
                .Value ??
            throw new ApplicationException("UserAccount context is not available");

        if(!Guid.TryParse(userAccountIdValue, out var userAccountIdGuid))
            throw new ApplicationException("UserAccount context is in the wrong format");

        return new UserAccountId(userAccountIdGuid);
    }
}