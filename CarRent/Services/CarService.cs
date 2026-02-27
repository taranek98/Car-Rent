using CarRent.Interfaces;
using CarRent.Models;
using CarRent.DataBase;
using Microsoft.EntityFrameworkCore;
using CarRent.ViewModels;
using CarRent.Helper;
namespace CarRent.Services;
public class CarService : BaseService, ICarService
{
    public CarService(AppDbContext context, ILogger<CarService> logger)
        :base(context, logger){}

    public async Task<ServiceResult> AddAsync(string vin, string mark, string model, 
        string color, string fuel, decimal prize)
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
            var result = await _context.SaveChangesAsync();
            if(result == DatabaseNoChanges)
            {
                _logger.LogError("Save changes is not work");
                return ServiceResult.Failure("Coś poszło nie tak");       
            }
            return ServiceResult.Success("Auto dodane");   

        }   
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Add Car");
            return ServiceResult.Failure("Błąd techniczny");;
        }
    }

    public async Task<ServiceResult> DeleteAsync(string vin)
    {
        try
        {
            var car = await GetCarByIdAsync(vin);
            _context.Cars.Remove(car);
            var result = await _context.SaveChangesAsync();
            if(result == 0)
            {
                _logger.LogError("Save changes is not work");
               return ServiceResult.Failure("Nie można usunąć podanego samochodu"); 
            }
            return ServiceResult.Success("Samochód został usunięty"); 
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Save database Error in delete car");
            return ServiceResult.Failure("Nie można usunąć podanego samochodu"); ;
        }
    }

    public async Task<ServiceResult<IList<CarView>>> InfoAllAvailableAsync()
    {
        IList<CarView> carsAvailable = new List<CarView>();
        var cars = await _context.Cars.ToListAsync();
        foreach(var car in cars)
        {
            if(car.User == null) 
            {
                carsAvailable.Add(new CarView(car));
            }
        }
        return ServiceResult<IList<CarView>>.Success(carsAvailable,"Dostępne samochody");
    }
    
}

