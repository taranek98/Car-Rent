using CarRent.Models;

namespace CarRent.ViewModels;
public class UserAdminView
{   
    public string Id {get; set;}
    public long Pesel {get; set;}
    public string Name {get; set;}
    public string LastName {get; set;}
    public IList<string>? VINs {get; set;}
    public string idCart{get; set;} 
    public decimal AccountBalance{get; set;}

    public UserAdminView(User user)
    {
        Id = user.Id;
        Pesel = user.Pesel;
        Name = user.Name;
        LastName = user.LastName;
        VINs = new List<string>();
        AccountBalance = user.AccountBalance;
        foreach (var car in user.Cars)
        {
            VINs.Add(car.VIN);
        }
        idCart = user.Cart.Id;
    }
}