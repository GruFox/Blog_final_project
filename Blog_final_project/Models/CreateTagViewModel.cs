using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class CreateTagViewModel
{
    [Required(ErrorMessage = "Необходимо ввести название тега")]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
}
