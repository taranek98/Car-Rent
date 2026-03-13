using CarRent.DataBase;
using CarRent.Helper;
using CarRent.Interfaces;
using CarRent.Models;
using CarRent.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Services;

public class CartService : BaseService, ICartInterface
{
    public CartService(AppDbContext context, ILogger<CartService> logger)
    :base(context, logger){}
    public async Task<ServiceResult> AddCarAsync(string vin, string cartId)
    {
        try
        {
            var newCar = await GetCarByIdAsync(vin);
            if(newCar.User != null)
            {
                _logger.LogError("Car is added to other user");
                return ServiceResult.Failure("Samochód jest niedostępny");
            }
            var cart = await GetCartWithCarsByIdAsync(cartId);
            foreach (var carInCart in cart.Cars)
            {
                if(carInCart.VIN == newCar.VIN)
                {
                    _logger.LogError("Add twice the same car to cart");
                    return ServiceResult.Failure("Samochód jest już w koszyku");  
                }
            }
            cart.Cars.Add(newCar);
            cart.Totality += newCar.PrizeForDay;
            var result = await _context.SaveChangesAsync();
            if(result == DatabaseNoChanges)
            {
                _logger.LogError("Database save Error");
                return ServiceResult.Failure("Błąd techniczny");    
            }
            return ServiceResult.Success("Auto dodane do koszyka");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Add Car to Cart");
            return ServiceResult.Failure("Błąd techniczny");
        }
    }

    public async Task<ServiceResult> BuyCarsAsync(string userId, string cartId)
    {
        try
        {
            var user = await GetUserWithCarsByIdAsync(userId);
            var cart = await GetCartWithCarsByIdAsync(cartId);
            if(cart.Cars.Count == 0)
            {
                _logger.LogError("Cart is empty");
                return ServiceResult.Failure("Brak aut w koszyku");
            }
            if(user.AccountBalance - cart.Totality < 0)
            {
                _logger.LogError("User have not enough funds");
                return ServiceResult.Failure("Nie masz wystarczajacej liczby środków na koncie");
            }
            user.AccountBalance -= cart.Totality;
            foreach(var car in cart.Cars.ToList())
            {
                user.Cars!.Add(car);
            }
            cart.Cars.Clear();
            var result = await _context.SaveChangesAsync();
            if(result == DatabaseNoChanges)
            {
                _logger.LogError("Database save Error");
                return ServiceResult.Failure("Coś poszlo nie tak");  
            }
            return ServiceResult.Success("Transakcja zaakceptowana");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Buy Car");
            return ServiceResult.Failure("Coś poszło nie tak");
        }  
    }

    public async Task<ServiceResult<IList<CarView>>> GetCarsAsync(string cartId)
    {
        try
        {
            var cart = await GetCartWithCarsByIdAsync(cartId);
            var carViewList = new List<CarView>();
            foreach (var car in cart.Cars)
            {
                carViewList.Add(new CarView(car));
            }
            return  ServiceResult<IList<CarView>>.Success(carViewList, "Operacja powiodła się");    
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Buy Car");
            return ServiceResult<IList<CarView>>.Failure("Coś poszło nie tak");
        }
    }
}