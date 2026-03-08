using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class CreateRoleViewModel
{
    [Required(ErrorMessage = "Введите название роли")]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Опишите новую роль")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = null!;
}
