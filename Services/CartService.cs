using CarRent.DataBase;
using CarRent.Interfaces;
using CarRent.Models;
using CarRent.ViewModels;
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

    public async Task<IList<CarView>> GetCars(string cartId)
    {
        try
        {
            var cart = await _context.Carts.Include(c => c.Cars).FirstOrDefaultAsync(c => c.Id == cartId);
            if(cart == null)
            {
                return null;
            }

            return new CartView(cart);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}