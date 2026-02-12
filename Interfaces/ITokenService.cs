using CarRent.Models;

namespace CarRent.Interfaces;
public interface ITokenService
{
    string CreateToken(User user, IList<string> roles);
}