using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class EditRoleViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [Required]
    [Display(Name = "Описание")]
    public string Description { get; set; } = null!;
}
