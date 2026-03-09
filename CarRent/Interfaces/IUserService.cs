using System.ComponentModel.DataAnnotations;
using CarRent.Helper;
using CarRent.Models;
using CarRent.ViewModels;
namespace CarRent.Interfaces;

public interface IUserService
{
    Task<ServiceResult> AddAsync(string pesel, string name, string lastname, 
        string password, string email);
    Task<ServiceResult<UserView>> InfoAsync(string id);
    Task<ServiceResult<IList<UserAdminView>>> InfoAllAsync();
    Task<ServiceResult<string>> LoginAsync(string userName, string password);
    Task<ServiceResult> DeleteAsync(string id);
    Task<ServiceResult> AddAdminToRoleAsync(string id);
    Task<ServiceResult> AddFundsAsync(string id, decimal funds);
}