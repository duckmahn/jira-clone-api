namespace managementapp.Data.Models;

public class Kanban
{
    public Guid Id { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; }
    public ICollection<Todolist> Todolists { get; set; } = new List<Todolist>();
    public Guid ProjectId { get; set; }
}
