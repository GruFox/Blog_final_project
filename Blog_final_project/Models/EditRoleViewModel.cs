using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class EditRoleViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название роли")]
    [StringLength(100, ErrorMessage = "Название роли слишком длинное")]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Опишите новую роль")]
    [StringLength(500, ErrorMessage = "Описание роли слишком длинное")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = null!;
}
