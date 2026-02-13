using CarRent.DataBase;
using CarRent.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            var car = await _context.Cars.FindAsync(vin);
            var cart = await _context.Carts.Include(c => c.Cars).FirstOrDefaultAsync(c => c.Id == cartId);
            if(car == null || cart == null || car.User != null)
            {
                return false;
            }
            cart.Cars.Add(car);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}