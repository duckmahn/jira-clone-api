namespace managementapp.Data.Models;

public class ProjectUser
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public Guid UserId { get; set; }
    public Users User { get; set; }
}
