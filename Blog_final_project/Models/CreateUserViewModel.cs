using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class CreateUserViewModel
{
    [Required]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = null!;

    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    public List<int> SelectedRoleIds { get; set; } = new();

    public List<Role> AllRoles { get; set; } = new();
}
