namespace CarRent.Interfaces;

public interface ICartInterface
{
    Task<bool> AddCar(string vin, string cartId);
}