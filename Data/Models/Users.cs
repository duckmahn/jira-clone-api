namespace managementapp.Data.Models;

public class Users
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public string Password { get; set; }
    public ICollection<ProjectUser>? ProjectUsers { get; }

}
