namespace managementapp.Data.Models;

public class Project
{
    public Guid Id { get; set; }//hidden
    public string Title { get; set; }
    public ICollection<Kanban>? Kanbans { get; set; }
    public ICollection<ProjectUser>? ProjectUsers { get; }
    public Guid AdminId { get; set; }
}
