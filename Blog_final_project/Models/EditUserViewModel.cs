namespace Blog_final_project.Models;

public class EditUserViewModel
{
    public int Id { get; set; }
    public List<Role> Roles { get; set; } = new();
    public List<int> SelectedRoleIds { get; set; } = new();
    public RegisterViewModel RegistrationValues { get; set; } = new();
    public bool CanEditRoles { get; set; }
}
