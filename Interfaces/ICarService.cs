using CarRent.Models;

namespace CarRent.Interfaces;

public interface ICarService
{
    Task<bool> AddAsync(string VIN, string Mark, string Model, 
        String Color, FuelType fuel, float Prize);
    Task<IList<Car>> InfoAllAvailableAsync();
    Task<bool> DeleteAsync(string VIN);
}