using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo_ASP__SignalR_JWT.Tools
{
    public class TokenManager
    {
        private IConfiguration _Config;

        public TokenManager(IConfiguration config)
        {
            _Config = config;
        }

        public string GenerateToken(string pseudo)
        {
            // Génération des credientials
            byte[] secret = Encoding.UTF8.GetBytes(_Config["Token:Secret"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secret);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            // Claim ← Objet avec la valeur à injecté dans le token
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, pseudo)
            };

            // Création du token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _Config["Token:Issuer"],
                audience: _Config["Token:Audience"],
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(60)
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
