using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TogetherBoardsApp.Backend.Infrastructure.Token;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly TokenOptions _tokenOptions;
    private readonly RsaKeysProvider _rsaKeysProvider;

    public JwtBearerOptionsSetup(
        IOptions<TokenOptions> tokenServiceOptions,
        RsaKeysProvider rsaKeysProvider)
    {
        _tokenOptions = tokenServiceOptions.Value;
        _rsaKeysProvider = rsaKeysProvider;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _tokenOptions.AccessTokenOptions.Issuer,
            ValidAudience = _tokenOptions.AccessTokenOptions.Audience,
            IssuerSigningKey = _rsaKeysProvider.RsaPublicSecurityKey,
            ClockSkew = TimeSpan.FromSeconds(5)
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}