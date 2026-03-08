using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class EditTagViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
}
