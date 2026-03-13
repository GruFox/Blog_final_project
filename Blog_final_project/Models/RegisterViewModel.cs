using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите имя")]
    [StringLength(50, ErrorMessage = "Имя слишком длинное")]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Введите фамилию")]
    [StringLength(50, ErrorMessage = "Фамилия слишком длинная")]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Введите email")]
    [StringLength(100, ErrorMessage = "email слишком длинный")]
    [Display(Name = "E-mail")]
    public string EmailReg { get; set; } = null!;

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [StringLength(50, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
    public string PasswordReg { get; set; } = null!;

    [Required]
    [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; } = null!;
}
