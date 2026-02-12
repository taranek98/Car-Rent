using System.ComponentModel.DataAnnotations;
using CarRent.Models;
namespace CarRent.Interfaces;

public interface IUserService
{
    Task<bool> AddAsync(int pesel, string name, string lastname, 
        string password, string email);
    Task<User> InfoAsync(string id);
    Task<IList<User>> InfoAllAsync();
    Task<string> LoginAsync(string userName, string password);
    Task<bool> DeleteAsync(string id);
    Task<bool> AddAdminToRoleAsync(string id);
}