using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Fragment.NetSlum.Server.Authentication.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fragment.NetSlum.Server.Authentication;

public class DiscordJwtTokenHandler : JwtSecurityTokenHandler
{
    private readonly IOptions<DiscordAuthOptions> _options;

    public DiscordJwtTokenHandler(IOptions<DiscordAuthOptions> options)
    {
        _options = options;
    }

    public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        var iv = MD5.HashData(Encoding.UTF8.GetBytes(_options.Value.JwtSecret));

        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = Encoding.UTF8.GetBytes(_options.Value.JwtSecret);
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        return base.ValidateToken(token, validationParameters, out validatedToken);
    }
}
