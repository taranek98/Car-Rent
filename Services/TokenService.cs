using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRent.Interfaces;
using CarRent.Models;
using Microsoft.IdentityModel.Tokens;
namespace CarRent.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
    }
    public string CreateToken(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptior = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(claims),
          SigningCredentials = creds,
          Expires = DateTime.Now.AddHours(1),
          Issuer = _config["Jwt:Issuer"],
          Audience = _config["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptior);
        return tokenHandler.WriteToken(token);
    }
}