using CarRent.Interfaces;
using CarRent.Models;
using CarRent.DataBase;
using Microsoft.EntityFrameworkCore;
namespace CarRent.Services;
public class CarService : ICarService
{
    private readonly AppDbContext _context;
    public CarService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(string vin, string mark, string model, 
        string color, FuelType fuel, float prize)
    {
        try
        {
            _context.Add(new Car()
            {
                VIN = vin, 
                Mark = mark, 
                Model = model,
                Color = color,
                Fuel = fuel,
                PrizeForDay = prize
            });
            await _context.SaveChangesAsync();
            return true;
        }   
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string VIN)
    {
        try
        {
            var car = await _context.Cars.FindAsync(VIN);
            if(car == null)
            {
               return false; 
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IList<Car>> InfoAllAvailableAsync()
    {
        IList<Car> carsAvailable = new List<Car>();
        var cars = await _context.Cars.ToListAsync();
        foreach(var car in cars)
        {
            if(car.User == null) 
            {
                carsAvailable.Add(car);
            }
        }
        return carsAvailable;
    }
}

