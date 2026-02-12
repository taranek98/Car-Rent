using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Models;
public enum FuelType
{
    Gas = 1,
    Diesel = 2,
    LPG = 3
}
public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public string VIN {get; set;}
    [Required]
    public string Mark {get; set;}
    [Required]
    public string Model {get; set;}
    [Required]
    public string Color {get; set;}
    [Required]
    public FuelType Fuel {get; set;}
    [Required]
    public float PrizeForDay {get; set;}
    public string? IdUser {get; set;}
    // [ForeignKey("IdUser")]
    public User? User {get; set;}
    public string? IdCart {get; set;}
    public Cart? Cart {get; set;}
}