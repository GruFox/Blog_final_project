using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class CreateTagViewModel
{
    [Required(ErrorMessage = "Необходимо ввести название тега")]
    [StringLength(50, ErrorMessage = "Название тега слишком длинное")]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
}
