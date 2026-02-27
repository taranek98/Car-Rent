using CarRent.Models;

namespace CarRent.ViewModels;

public class CartView
{
    public decimal Totality {get; set;}
    public IList<CarView> Cars {get; set;} = new List<CarView>();
    public CartView(Cart cart)
    {
        Totality = cart.Totality;
        foreach(var car in cart.Cars)
        {
            Cars.Add(new CarView(car));
        }
    }
}