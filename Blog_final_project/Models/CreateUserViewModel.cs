using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Необходимо ввести имя")]
    [StringLength(50, ErrorMessage = "Имя слишком длинное")]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо ввести фамилию")]
    [StringLength(50, ErrorMessage = "Фамилия слишком длинная")]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо ввести email")]
    [StringLength(100, ErrorMessage = "email слишком длинный")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо ввести пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [StringLength(50, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
    [MinLength(5)]
    public string Password { get; set; } = null!;

    public List<int> SelectedRoleIds { get; set; } = new();

    public List<Role> AllRoles { get; set; } = new();
}
