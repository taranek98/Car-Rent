using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Models;
public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public required string VIN {get; set;}
    [Required]
    public required string Mark {get; set;}
    [Required]
    public required string Model {get; set;}
    [Required]
    public required string Color {get; set;}
    [Required]
    public required string Fuel {get; set;}
    [Required]
    public decimal PrizeForDay {get; set;}
    public string? IdUser {get; set;}
    public User? User {get; set;}
    public string? IdCart {get; set;}
    public Cart? Cart {get; set;}
}