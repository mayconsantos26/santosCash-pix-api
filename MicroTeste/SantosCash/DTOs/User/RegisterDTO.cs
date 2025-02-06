using System.ComponentModel.DataAnnotations;

namespace DTOs.User;

public class RegisterDTO
{
    [Required(ErrorMessage = "Nome de usuário é obrigatório")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email é obrigatória")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Role(Papel) é obrigatória")]
    public string? Role { get; set; }
}
