namespace managementapp.Data.Models;

public class Kanban
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string StatusList { get; set; }
    public ICollection<Todolist> Todolists { get; set; } = new List<Todolist>();
    public Guid ProjectId { get; set; }
}
