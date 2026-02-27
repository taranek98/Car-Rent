using System.Text.Json.Serialization;
using CarRent.Models;

namespace CarRent.ViewModels;
public class UserView
{   
    public long Pesel {get; set;}
    public string Name {get; set;}
    public string LastName {get; set;}
    [JsonPropertyName("vins")]
    public  IList<string>? VINs {get; set;}
    public decimal AccountBalance {get; set;}

    public UserView(User user)
    {
        Pesel = user.Pesel;
        Name = user.Name;
        LastName = user.LastName;
        VINs = new List<string>();
        AccountBalance = user.AccountBalance;
        foreach (var car in user.Cars)
        {
            VINs.Add(car.VIN);
        }
    }
}