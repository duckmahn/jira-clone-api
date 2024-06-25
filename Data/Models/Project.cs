namespace managementapp.Data.Models;

public class Project
{
    public Guid Id { get; set; }//hidden
    public string? Title { get; set; }
    public ICollection<Todolist> Todolists { get; set; }

    public ICollection<Users> UserId { get; set; }
    public Guid AdminId { get; set; }
}
