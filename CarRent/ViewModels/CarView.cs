using CarRent.Models;

namespace CarRent.ViewModels;
public class CarView
{
    public string VIN {get; set;}
    public string Mark {get; set;}
    public string Model {get; set;}
    public string Color {get; set;}
    public string Fuel {get; set;}
    public decimal PrizeForDay {get; set;}

    public CarView(Car car)
    {
        VIN = car.VIN;
        Mark = car.Mark;
        Model = car.Model;
        Color = car.Color;
        Fuel = car.Fuel;
        PrizeForDay = car.PrizeForDay;
    }
}