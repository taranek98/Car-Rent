using CarRent.Models;
namespace CarRent.Interfaces;

public interface ICartInterface
{
    Task<bool> AddCar(string vin, string cartId);
    Task<IList<Car>> GetCars(string cartId);
}