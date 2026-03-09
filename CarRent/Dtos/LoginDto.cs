using System.ComponentModel.DataAnnotations;

namespace CarRent.Dtos;
public class LoginDto
{
    [Required(ErrorMessage = "Email jest wymagany")]
    public required string Email {get; set;}
    [Required(ErrorMessage = "Hasło jest wymagane")]
    public required string Password {get; set;}
}