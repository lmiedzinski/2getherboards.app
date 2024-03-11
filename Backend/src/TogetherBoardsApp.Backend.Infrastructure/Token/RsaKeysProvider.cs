using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TogetherBoardsApp.Backend.Infrastructure.Token;

public sealed class RsaKeysProvider
{
    public RsaKeysProvider(IOptions<TokenOptions> tokenOptions)
    {
        RsaPrivateSecurityKey = PreparePrivateKey(tokenOptions.Value.AccessTokenOptions.PrivateRsaCertificate);
        RsaPublicSecurityKey = PreparePublicKey(tokenOptions.Value.AccessTokenOptions.PublicRsaCertificate);
    }

    public RsaSecurityKey RsaPrivateSecurityKey { get; private set; }
    public RsaSecurityKey RsaPublicSecurityKey { get; private set; }

    private static RsaSecurityKey PreparePrivateKey(string base64Key)
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(
            Convert.FromBase64String(base64Key),
            out _);
        return new RsaSecurityKey(rsa);
    }
    
    private static RsaSecurityKey PreparePublicKey(string base64Key)
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(
            Convert.FromBase64String(base64Key),
            out _);
        return new RsaSecurityKey(rsa);
    }
}