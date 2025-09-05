using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infraestructure.Service
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                throw new InvalidOperationException("Invalid secret Key");

            var privateKey = Encoding.UTF8.GetBytes(key);//Converte para um array de Bytes

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                                     SecurityAlgorithms.HmacSha256Signature); //Criar crendencias de assinatura

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").
                                                    GetValue<double>("TokenValidityInMinutes")),
                Audience = _config.GetSection("JWT")
                                  .GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128]; //Armazenar bytes aleatórios de forma seguro

            using var randomNumberGenerator = RandomNumberGenerator.Create(); //randomNumberGenerator é uma instancia dese Create

            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);//Convertando os bytes aleatórios para uma representação de string
            // token legível, fácil de armazenar ou de transmitir

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid key");

            var tokenValidationParameters = new TokenValidationParameters //Parametros de validação do token expirado
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false //expirado
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                                                        out SecurityToken securityToken);//parametro de saída

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                              !jwtSecurityToken.Header.Alg.Equals(
                               SecurityAlgorithms.HmacSha256,
                               StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
    }
}
