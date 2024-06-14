namespace managementapp.Data.Models;

public class Todolist
{
    public Guid Id { get; set; }//hidden
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }//hidden
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
}
