using CarRent.Models;

namespace CarRent.Interfaces;

public interface ICarService
{
    Task<bool> AddAsync(string VIN, string Mark, string Model, 
        string Color, string fuel, float Prize);
    Task<IList<Car>> InfoAllAvailableAsync();
    Task<bool> DeleteAsync(string VIN);
}