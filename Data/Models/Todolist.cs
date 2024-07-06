namespace managementapp.Data.Models;

public class Todolist
{
    public Guid Id { get; set; }//hidden
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; }
    public DateTime CreatedAt { get; set; }//hidden
    public DateTime UpdatedAt { get; set; }//hidden

    public DateTime ExpiredAt { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}
