using CarRent.Helper;
using CarRent.Models;
using CarRent.ViewModels;

namespace CarRent.Interfaces;

public interface ICarService
{
    Task<ServiceResult> AddAsync(string vin, string mark, string model, 
        string color, string fuel, decimal prize);
    Task<ServiceResult<IList<CarView>>> InfoAllAvailableAsync();
    Task<ServiceResult> DeleteAsync(string vin);
}