using System.ComponentModel.DataAnnotations;

namespace DTOs.User;

public class RegisterUserDTO
{
    [Required(ErrorMessage = "Nome de usuário é obrigatório")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(9, ErrorMessage = "Senha deve conter no minimo de 9 caracteres")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Role(Papel) é obrigatória")]
    [RegularExpression("Admin|User", ErrorMessage = "Role(Papel) inválida")]
    public string? Role { get; set; }
}
