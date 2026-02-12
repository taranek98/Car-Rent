using CarRent.DataBase;
using CarRent.Interfaces;

namespace CarRent.Services;

public class CartService : ICartInterface
{
    private readonly AppDbContext _context;
    public CartService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> AddCar(string vin, string cartId)
    {
        try
        {
            var car = await _context.Cars.Include(c => c.cart).FindAsync(vin);
            var cart = await _context.Carts.FindAsync(cartId);
            if(car == null || cart == null || car.User != null)
            {
                return false;
            }
            cart.Cars.Add(car);
            return true;
        }
        catch
        {
            return false;
        }
    }
}