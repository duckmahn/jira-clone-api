namespace managementapp.Data.Models;

public class Todolist
{
    public Guid Id { get; set; }//hidden
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }//hidden
    public DateTime UpdatedAt { get; set; }
    public int UserId { get; set; }
}
