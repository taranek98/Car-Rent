using CarRent.DataBase;
using CarRent.Helper;
using CarRent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CarRent.Services;
public abstract class BaseService
{
    protected readonly AppDbContext _context;
    protected readonly ILogger _logger;
    protected const int DatabaseNoChanges = 0;

    public BaseService(AppDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            _logger.LogError($"User with {id} does not exist");
            throw new ArgumentNullException();
        }
        return user;
    }
    public async Task<Car> GetCarByIdAsync(string id)
    {
        var car = await _context.Cars.FindAsync(id);
        if(car == null)
        {
            _logger.LogError($"Car with {id} does not exist");
            throw new ArgumentNullException();
        }
        return car;
    }
    public async Task<User> GetUserWithCarsByIdAsync(string id)
    {
        var user = await _context.Users.Include(u => u.Cars).FirstOrDefaultAsync(u => u.Id == id);
        if(user == null || user.Cars == null)
        {
            _logger.LogError($"User with {id} or user cars does not exist");
            throw new ArgumentNullException();
        }
        return user;
    } 
    public async Task<User> GetUserWithCarsAndCartByIdAsync(string id)
    {
        var user = await _context.Users.Include(u => u.Cars).Include(u => u.Cart).FirstOrDefaultAsync(u => u.Id == id);
        if(user == null || user.Cars == null || user.Cart == null)
        {
            _logger.LogError($"User with {id} or user Cars or Cart does not exist");
            throw new ArgumentNullException();
        }
        return user;
    } 
    public async Task<IList<User>> GetUsersWithCartAndCarAsync()
    {
        return  await _context.Users.Include(u => u.Cars).Include(u => u.Cart).ToListAsync();
    }
    public async Task<Cart> GetCartWithCarsByIdAsync(string id)
    {
        var cart = await _context.Carts.Include(c => c.Cars).FirstOrDefaultAsync(c => c.Id == id);
        if(cart == null)
        {
            _logger.LogError($"Cart with {id} does not exist");
            throw new ArgumentNullException();
        }
        return cart;
    }
    public void LogIdentityAsync(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            _logger.LogError($"Code: {error.Code}, Description: {error.Description}");
        }
    }
    public void RemoveUserCarConnection(User user)
    {
        if(user.Cars == null)
        {
            _logger.LogError("List of Cars is null");
            throw new ArgumentNullException();
        }
        foreach (var car in user.Cars)
        {
            car.IdUser = null;
        }
    }
    public void RemoveUser(User user)
    {
        _context.Users.Remove(user);
    }
    public void RemoveCart(Cart cart)
    {
        _context.Carts.Remove(cart);
    }
    public void RemoveCar(Car car)
    {
        _context.Cars.Remove(car);
    }
}