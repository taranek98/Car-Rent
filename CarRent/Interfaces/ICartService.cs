using CarRent.Helper;
using CarRent.Models;
using CarRent.ViewModels;
namespace CarRent.Interfaces;

public interface ICartInterface
{
    Task<ServiceResult> AddCarAsync(string vin, string cartId);
    Task<ServiceResult<IList<CarView>>> GetCarsAsync(string cartId);
    Task<ServiceResult> BuyCarsAsync(string userid, string cartId);
}