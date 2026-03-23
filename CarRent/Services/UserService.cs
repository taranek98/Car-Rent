using CarRent.Interfaces;
using CarRent.DataBase;
using CarRent.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CarRent.ViewModels;
using CarRent.Helper;
namespace CarRent.Services;
public class UserService : BaseService, IUserService
{
    
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager,AppDbContext context, ITokenService tokenService, ILogger<UserService> logger)
        :base (context, logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<ServiceResult> AddAdminToRoleAsync(string id)
    {
        try
        {
            var user = await GetUserByIdAsync(id);
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if(!result.Succeeded)
            {
                LogIdentityAsync(result);
                return ServiceResult.Failure("Nie można nadać roli");
            }
            return ServiceResult.Success("Rola nadana");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Add Admin");
            return ServiceResult.Failure();
        }
    }

    public async Task<ServiceResult> AddAsync(string pesel, string name, string lastName, 
        string password, string email)
    {
        try
        {
            User newUser = new User()
            {
                Pesel = pesel,
                Name = name,
                LastName = lastName,
                Email = email,
                UserName = email,
                Cart = new Cart(),
                Cars = new List<Car>(),
                AccountBalance = 0
            };
            newUser.Cart.User = newUser;
            var result = await _userManager.CreateAsync(newUser, password);
            if(!result.Succeeded)
            {
                LogIdentityAsync(result);
                return ServiceResult.Failure("Dodawanie użytkowanika nie powiodło się");
            }
            result = await _userManager.AddToRoleAsync(newUser, "User");
            if(!result.Succeeded)
            {
                LogIdentityAsync(result);
                return ServiceResult.Failure("Dodawanie użytkowanika nie powiodło się");
            }
            return ServiceResult.Success("Użytkownik został dodany");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Add User");
            return ServiceResult.Failure();
        }
    }

    public async Task<ServiceResult> DeleteAsync(string id)
    {
        try
        {
            var user = await GetUserWithCarsAndCartByIdAsync(id);
            RemoveUserCarConnection(user);
            RemoveCart(user.Cart!);
            RemoveUser(user);
            var result = await _context.SaveChangesAsync();
            if(result == DatabaseNoChanges)
            {
                _logger.LogError("Database Save Error");
                return ServiceResult.Failure("Nie można usunąć użytkownika");
            }
            return ServiceResult.Success("Użytkonwik usunięty");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Delete User");
            return ServiceResult.Failure();
        }
    }

    public async Task<ServiceResult<UserView>> InfoAsync(string id)
    {
        var user = await GetUserWithCarsByIdAsync(id);
        return ServiceResult<UserView>.Success(new UserView(user), "Dane użytkownika");
    }
    public async Task<ServiceResult<IList<UserAdminView>>> InfoAllAsync()
    {
        var users = await GetUsersWithCartAndCarAsync();
        var usersView = new List<UserAdminView>();
        foreach (var user in users)
        {
            usersView.Add(new UserAdminView(user));
        }
        return ServiceResult<IList<UserAdminView>>.Success(usersView, "Lista użytkowników");
    }

    public async Task<ServiceResult<string>> LoginAsync(string userName, string password)
    {
        try
        {   
            var user = await _userManager.FindByEmailAsync(userName);
            if(user == null)
            {
                _logger.LogError($"User with name:{userName} does not exist");
                return ServiceResult<string>.Failure("Nie poprawne dane logowania");
            }
            var ifCorrectPassword = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if(!ifCorrectPassword.Succeeded)
            {
                _logger.LogError($"Incorrect password");
                return ServiceResult<string>.Failure("Nie poprawne dane logowania");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if(roles.Count == 0)
            {
                _logger.LogError($"Roles are empty");
                return ServiceResult<string>.Failure("Błąd techniczny");
            }
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.IdUser == user.Id);
            if(cart == null)
            {
                _logger.LogError($"Cart does not exist");
                return ServiceResult<string>.Failure("Błąd techniczny");
            }
            var token = _tokenService.CreateToken(user, roles, cart.Id);
            return ServiceResult<string>.Success(token, "Logowanie zakończono sukcesem");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Login");
            return ServiceResult<string>.Failure();
        }
    }

    public async Task<ServiceResult> AddFundsAsync(string id, decimal funds)
    {
        try
        {
            if(funds <= 0)
            {
                 _logger.LogError("Try add negative funds");
                return ServiceResult.Failure("Środki musza być liczbą dodatnią");
            }
            var user = await GetUserByIdAsync(id);
            user.AccountBalance += funds;
            var result = await _context.SaveChangesAsync();
            if(result == 0)
            {
                _logger.LogError("Database Save Error");
                return ServiceResult.Failure("Nie udało się dodać środków");
            }
            return ServiceResult.Success("Środki dodano");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Add funds");
            return ServiceResult.Failure();
        }
    }

    public async Task<ServiceResult> ReturnCarAsync(string id, string vin)
    {
        try
        {
            var user = await GetUserWithCarsByIdAsync(id);
            var car = await GetCarByIdAsync(vin);
            user.Cars!.Remove(car);
            var result = await _context.SaveChangesAsync();
            if(result == 0)
            {
                _logger.LogError("Database Save Error");
                return ServiceResult.Failure("Nie udało się zwrócić Auta");
            }
            return ServiceResult.Success("Auto zwrócono");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Critical Error in Retrun car");
            return ServiceResult.Failure();
        }
    }
}